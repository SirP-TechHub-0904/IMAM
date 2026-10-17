#!/bin/bash
set -e

APP_DIR="/var/www/imam"
SERVICE_NAME="imam.service"
APP_EXEC="IMAM.UI"

SUDO=""
if [ "$(id -u)" -ne 0 ]; then
  SUDO="sudo"
fi

CURRENT_USER=$(whoami)

echo "=================================================="
echo ">>> Deploying IMAM Web Application"
echo ">>> User: $CURRENT_USER | Target Directory: $APP_DIR"
echo "=================================================="

# 1. Ensure target directory exists and has correct ownership
$SUDO mkdir -p "$APP_DIR"
$SUDO chown -R "$CURRENT_USER":"$CURRENT_USER" "$APP_DIR"

# 2. Back up existing .env if this is a subsequent deployment
PRESERVED_ENV=false
if [ -f "$APP_DIR/.env" ]; then
  echo ">>> Existing .env found - preserving production configuration..."
  cp "$APP_DIR/.env" /tmp/imam_existing.env
  PRESERVED_ENV=true
fi

# 3. Extract new release package
if [ -f "/tmp/app-package.tar.gz" ]; then
  echo ">>> Extracting new release package to $APP_DIR..."
  tar -xzf /tmp/app-package.tar.gz -C "$APP_DIR"
  rm -f /tmp/app-package.tar.gz
fi

# Make binary executable
chmod +x "$APP_DIR/$APP_EXEC" 2>/dev/null || true

# 4. Handle .env on first deploy vs subsequent deploys
if [ "$PRESERVED_ENV" = true ]; then
  echo ">>> Restoring preserved production .env file..."
  cp /tmp/imam_existing.env "$APP_DIR/.env"
  rm -f /tmp/imam_existing.env
else
  # First deployment
  if [ -f "$APP_DIR/.env" ]; then
    echo ">>> First deploy: .env was moved with the deployment package!"
  else
    echo ">>> First deploy: Initializing default .env configuration..."
    cat << 'EOF' > "$APP_DIR/.env"
# ASP.NET Core Environment
ASPNETCORE_ENVIRONMENT=Production

# Database Connection String
ConnectionStrings__AppConnection="Server=[REDACTED_SERVER_IP],1433;Database=db_aa9f2d_imam;User Id=user_db;Password=[REDACTED_DB_PASSWORD];TrustServerCertificate=True;MultipleActiveResultSets=true;"

# Postmark Email Service
PostmarkSettings__ServerToken="[REDACTED_POSTMARK_TOKEN]"

# AWS Configuration (S3 Storage)
AwsConfiguration__AWSAccessKey="[REDACTED_AWS_ACCESS_KEY]"
AwsConfiguration__AWSSecretKey="[REDACTED_AWS_SECRET_KEY]"
EOF
  fi
fi

chmod 600 "$APP_DIR/.env" 2>/dev/null || true

# 5. Port assignment (persisted in .port to prevent conflicts across apps)
PORT=""
if [ -f "$APP_DIR/.port" ]; then
  SAVED_PORT=$(cat "$APP_DIR/.port" | tr -d '[:space:]')
  if [[ "$SAVED_PORT" =~ ^[0-9]+$ ]]; then
    PORT="$SAVED_PORT"
    echo ">>> Reusing existing configured port: $PORT"
  fi
fi

if [ -z "$PORT" ]; then
  echo ">>> Detecting available port starting from 5000..."
  for p in $(seq 5000 5100); do
    if ! ss -tulpn 2>/dev/null | grep -q ":$p " && ! netstat -tuln 2>/dev/null | grep -q ":$p "; then
      PORT=$p
      echo "$PORT" > "$APP_DIR/.port"
      echo ">>> Assigned free port: $PORT (saved to $APP_DIR/.port)"
      break
    fi
  done
fi

if [ -z "$PORT" ]; then
  PORT=5008
  echo ">>> Defaulting to port $PORT"
fi

# 6. Configure systemd service
SERVICE_FILE="/etc/systemd/system/$SERVICE_NAME"
echo ">>> Creating/Updating systemd service: $SERVICE_FILE..."

$SUDO bash -c "cat << EOF > $SERVICE_FILE
[Unit]
Description=IMAM ASP.NET Core Razor Pages Web Application
After=network.target

[Service]
Type=notify
User=$CURRENT_USER
WorkingDirectory=$APP_DIR
ExecStart=$APP_DIR/$APP_EXEC
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=imam-app
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=ASPNETCORE_URLS=http://0.0.0.0:$PORT
EnvironmentFile=-$APP_DIR/.env

[Install]
WantedBy=multi-user.target
EOF"

# 7. Reload systemd, enable and restart service
echo ">>> Reloading systemd daemon..."
$SUDO systemctl daemon-reload

echo ">>> Enabling $SERVICE_NAME to start on system boot..."
$SUDO systemctl enable "$SERVICE_NAME"

echo ">>> Restarting $SERVICE_NAME..."
$SUDO systemctl restart "$SERVICE_NAME"

# 8. Check health status
sleep 4
if $SUDO systemctl is-active --quiet "$SERVICE_NAME"; then
  echo "=================================================="
  echo ">>> SUCCESS! $SERVICE_NAME is ACTIVE and RUNNING!"
  echo ">>> Application Port: $PORT"
  echo ">>> Local Address:    http://127.0.0.1:$PORT"
  echo ">>> Public Address:   http://$(curl -s https://ifconfig.me 2>/dev/null || echo 'YOUR_SERVER_IP'):$PORT"
  echo "=================================================="
else
  echo "=================================================="
  echo ">>> ERROR: $SERVICE_NAME failed to start!"
  echo ">>> Recent service logs:"
  echo "=================================================="
  $SUDO journalctl -u "$SERVICE_NAME" -n 35 --no-pager
  exit 1
fi
