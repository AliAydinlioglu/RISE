pipeline {
    agent any
    
    environment {
        // GitHub repository configuration
        GITHUB_REPO = 'https://github.com/HOGENT-RISE/dotnet-2526-tiao2.git'
        GITHUB_USERNAME = 'badramr1'
        
        // Application server configuration
        // Default value; will be auto-resolved from ops inventory if available
        APP_SERVER_HOST = '98.66.235.6'
        APP_SERVER_USER = 'deploy'
        APP_NAME = 'Rise.Server'
        APP_PORT = '5001'
        
        // Domain configuration for HTTPS (Cloudflare)
        APP_DOMAIN = 'campus.badrlab.xyz'
        
        // Build configuration
        DOTNET_VERSION = '9.0'
        BUILD_CONFIGURATION = 'Release'
        PUBLISH_DIR = 'publish'
        MAIN_PROJECT = 'src/Rise.Server/Rise.Server.csproj'
        
        // Deployment paths
        DEPLOY_BASE_PATH = '/opt/Rise.Server'
        CURRENT_PATH = '/opt/Rise.Server/current'
        RELEASES_PATH = '/opt/Rise.Server/releases'
    }
    
    options {
        // Keep build history
        buildDiscarder(logRotator(numToKeepStr: '99'))
        
        // Skip default checkout behavior
        skipDefaultCheckout()
    }
    
    stages {
        stage('Checkout') {
            steps {
                script {
                    echo "Checking out repository: ${GITHUB_REPO}"
                    
                    // Use credentials for private repository
                    checkout([
                        $class: 'GitSCM',
                        branches: [[name: '*/ops/main-cloud']],
                        doGenerateSubmoduleConfigurations: false,
                        extensions: [],
                        submoduleCfg: [],
                        userRemoteConfigs: [[
                            credentialsId: 'github-private-repo',
                            url: GITHUB_REPO
                        ]]
                    ])
                    
                    // Get commit information
                    sh '''
                        echo "Repository Information:"
                        echo "  - Repository: ${GITHUB_REPO}"
                        echo "  - Branch: $(git branch --show-current)"
                        echo "  - Commit: $(git rev-parse HEAD)"
                        echo "  - Author: $(git log -1 --pretty=format:'%an <%ae>')"
                        echo "  - Message: $(git log -1 --pretty=format:'%s')"
                    '''
                }
            }
        }

        // IPs are hardcoded via APP_SERVER_HOST in environment block per requirement
        
        stage('Build') {
            steps {
                script {
                    echo "Building .NET ${DOTNET_VERSION} application..."
                    
                    // Restore dependencies
                    sh "dotnet restore ${MAIN_PROJECT}"
                    
                    // Build the solution
                    sh "dotnet build ${MAIN_PROJECT} --configuration ${BUILD_CONFIGURATION} --no-restore"
                    
                    echo "Build completed successfully!"
                }
            }
        }
        
        stage('Test') {
            steps {
                script {
                    echo "Running tests..."
                    
                    // Run all tests in the solution
                    sh "dotnet test --configuration ${BUILD_CONFIGURATION} --no-build --verbosity normal --logger trx --results-directory TestResults"
                    
                    // Debug: List generated test result files
                    sh "echo 'Generated test result files:' && find TestResults -name '*.trx' -type f 2>/dev/null || echo 'No TRX files found'"
                    
                    echo "Tests completed successfully!"
                }
            }
            post {
                always {
                    // Publish test results using mstest step (only if TRX files exist)
                    script {
                        def trxFiles = sh(script: "find TestResults -name '*.trx' -type f 2>/dev/null | wc -l", returnStdout: true).trim()
                        if (trxFiles != "0") {
                            echo "Found ${trxFiles} TRX file(s), publishing test results..."
                            mstest testResultsFile: 'TestResults/*.trx'
                        } else {
                            echo "No TRX files found, skipping test result publishing"
                        }
                    }
                }
            }
        }
        
        stage('Publish') {
            steps {
                script {
                    echo "Publishing application for deployment..."
                    
                    // Clean publish directory
                    sh "rm -rf ${PUBLISH_DIR}"
                    
                    // Publish the application
                    sh "dotnet publish ${MAIN_PROJECT} --configuration ${BUILD_CONFIGURATION} --output ${PUBLISH_DIR} --no-build"
                    
                    echo "Application published to ${PUBLISH_DIR}"
                }
            }
        }
        
        stage('Deploy') {
            steps {
                script {
                    echo "Deploying to application server ${APP_SERVER_HOST}..."
                    
                    // Use Jenkins credentials for SSH authentication
                    withCredentials([sshUserPrivateKey(credentialsId: 'deploy-ssh-key', keyFileVariable: 'SSH_KEY')]) {
                    
                    // Create timestamped release directory
                    def timestamp = sh(script: "date +%Y%m%d%H%M%S", returnStdout: true).trim()
                    def releaseDir = "${RELEASES_PATH}/${timestamp}"
                    
                        // Prepare published config files before transfer (server and client)
                        sh """
                            if ls ${PUBLISH_DIR}/appsettings*.json >/dev/null 2>&1; then
                                sed -i "s|https://localhost:|http://${APP_SERVER_HOST}:|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|http://localhost:|http://${APP_SERVER_HOST}:|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|https://127.0.0.1:|http://${APP_SERVER_HOST}:|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|http://127.0.0.1:|http://${APP_SERVER_HOST}:|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|https://0.0.0.0:|http://${APP_SERVER_HOST}:|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|http://0.0.0.0:|http://${APP_SERVER_HOST}:|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|\"https://localhost\"|\"http://${APP_SERVER_HOST}\"|g" ${PUBLISH_DIR}/appsettings*.json || true
                                sed -i "s|\"http://localhost\"|\"http://${APP_SERVER_HOST}\"|g" ${PUBLISH_DIR}/appsettings*.json || true
                                # Do not modify FrontendUrl/BackendUrl in root appsettings here; CORS is set via environment
                                # Update DatabaseConnection to point to DB server (using perl for safe JSON manipulation)
                                perl -i -pe 's/"DatabaseConnection"\\s*:\\s*"[^"]*"/"DatabaseConnection": "server=98.66.235.220;port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none"/' ${PUBLISH_DIR}/appsettings.json || true
                                echo '=== VERIFY publish/appsettings.json DatabaseConnection ==='
                                grep -n "\"DatabaseConnection\"" ${PUBLISH_DIR}/appsettings.json || true
                            fi
                            if ls ${PUBLISH_DIR}/wwwroot/appsettings*.json >/dev/null 2>&1; then
                                sed -i "s|https://localhost:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|http://localhost:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|https://127.0.0.1:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|http://127.0.0.1:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|https://0.0.0.0:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|http://0.0.0.0:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|http://${APP_SERVER_HOST}:|https://${APP_DOMAIN}|g" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                # Explicit BackendUrl/FrontendUrl (no capture groups) - use HTTPS domain
                                sed -i "s|\"BackendUrl\"[[:space:]]*:[[:space:]]*\".*\"|\"BackendUrl\":\"https://${APP_DOMAIN}\"|" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                sed -i "s|\"FrontendUrl\"[[:space:]]*:[[:space:]]*\".*\"|\"FrontendUrl\":\"https://${APP_DOMAIN}\"|" ${PUBLISH_DIR}/wwwroot/appsettings*.json || true
                                echo '=== VERIFY publish/wwwroot/appsettings.json URLs ==='
                                head -n 50 ${PUBLISH_DIR}/wwwroot/appsettings.json || true
                            fi
                        """
                        
                        // Copy application files to server using SSH key
                        sh """
                            scp -i ${SSH_KEY} -o StrictHostKeyChecking=no -r ${PUBLISH_DIR}/* ${APP_SERVER_USER}@${APP_SERVER_HOST}:${releaseDir}/
                        """
                    
                        // Fix permissions and deploy application
                        sh """
                            ssh -i ${SSH_KEY} -o StrictHostKeyChecking=no ${APP_SERVER_USER}@${APP_SERVER_HOST} << 'EOF'
                                # Fix permissions on the release directory
                                sudo chown -R ${APP_SERVER_USER}:${APP_SERVER_USER} ${releaseDir}
                                sudo chmod -R 755 ${releaseDir}
                                
                                # Remove existing current directory
                                sudo rm -rf ${CURRENT_PATH}
                                
                                # Create new current directory and copy files
                                sudo mkdir -p ${CURRENT_PATH}
                                sudo cp -r ${releaseDir}/* ${CURRENT_PATH}/
                                sudo chown -R ${APP_SERVER_USER}:${APP_SERVER_USER} ${CURRENT_PATH}
                                sudo chmod -R 755 ${CURRENT_PATH}
                                
                                # Ensure application binds to all interfaces and not localhost
                                sudo mkdir -p /etc/systemd/system/${APP_NAME}.service.d
                                cat << 'EOC' | sudo tee /etc/systemd/system/${APP_NAME}.service.d/override.conf >/dev/null
                                [Service]
                                Environment=ASPNETCORE_ENVIRONMENT=Development
                                Environment=ASPNETCORE_URLS=http://0.0.0.0:${APP_PORT}
                                Environment=APP_SERVER_HOST=${APP_SERVER_HOST}
                                Environment=ConnectionStrings__DatabaseConnection=server=98.66.235.220;port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none
                                Environment=DatabaseConnection=server=98.66.235.220;port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none
                                Environment=FrontendUrl=https://${APP_DOMAIN}
                                WorkingDirectory=${CURRENT_PATH}
                                Restart=always
                                RestartSec=5
EOC
                                
                                # Optional: replace localhost with server IP in deployed appsettings files
                                if ls ${CURRENT_PATH}/appsettings*.json >/dev/null 2>&1; then
                                    # Force HTTP for local runs and map localhost-like hosts to APP_SERVER_HOST
                                    sudo sed -i "s|https://localhost:|http://${APP_SERVER_HOST}:|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|http://localhost:|http://${APP_SERVER_HOST}:|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|https://127.0.0.1:|http://${APP_SERVER_HOST}:|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|http://127.0.0.1:|http://${APP_SERVER_HOST}:|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|https://0.0.0.0:|http://${APP_SERVER_HOST}:|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|http://0.0.0.0:|http://${APP_SERVER_HOST}:|g" ${CURRENT_PATH}/appsettings*.json || true
                                    # Fallback: replace bare localhost hostnames with APP_SERVER_HOST
                                    sudo sed -i "s|\"https://localhost\"|\"http://${APP_SERVER_HOST}\"|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|\"http://localhost\"|\"http://${APP_SERVER_HOST}\"|g" ${CURRENT_PATH}/appsettings*.json || true
                                    sudo sed -i "s|localhost|${APP_SERVER_HOST}|g" ${CURRENT_PATH}/appsettings*.json || true
                                    # Update DatabaseConnection to point to DB server (using perl for safe JSON manipulation)
                                    sudo perl -i -pe 's/"DatabaseConnection"\\s*:\\s*"[^"]*"/"DatabaseConnection": "server=192.168.56.11;port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none"/' ${CURRENT_PATH}/appsettings.json || true
                                fi

                                # Update client-side appsettings in wwwroot (BackendUrl/FrontendUrl) - use HTTPS domain
                                if ls ${CURRENT_PATH}/wwwroot/appsettings*.json >/dev/null 2>&1; then
                                    sudo sed -i "s|https://localhost:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|http://localhost:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|https://127.0.0.1:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|http://127.0.0.1:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|https://0.0.0.0:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|http://0.0.0.0:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|http://${APP_SERVER_HOST}:|https://${APP_DOMAIN}|g" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    # Explicitly set BackendUrl and FrontendUrl to the HTTPS domain (no backrefs)
                                    sudo sed -i "s|\"BackendUrl\"[[:space:]]*:[[:space:]]*\".*\"|\"BackendUrl\":\"https://${APP_DOMAIN}\"|" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                    sudo sed -i "s|\"FrontendUrl\"[[:space:]]*:[[:space:]]*\".*\"|\"FrontendUrl\":\"https://${APP_DOMAIN}\"|" ${CURRENT_PATH}/wwwroot/appsettings*.json || true
                                fi
                                
                                # Reload systemd to apply drop-in
                                sudo systemctl daemon-reload
                                
                                # Stop existing service if running
                                sudo systemctl stop ${APP_NAME} || true
                                
                                # Start the service
                                sudo systemctl start ${APP_NAME}
                                sudo systemctl enable ${APP_NAME}
                                
                                echo '=== Deployment completed ==='
                                sudo systemctl is-active --quiet ${APP_NAME} && echo 'Service is running!' || echo 'Service failed to start'
EOF
                        """

                        echo "Deployment completed successfully!"
                    }
                }
            }
        }
        
        stage('Health Check') {
            steps {
                script {
                    echo "Performing health check..."
                    
                    // Wait a moment for service to start
                    sh "sleep 10"
                    
                    // Use Jenkins credentials for SSH authentication
                    withCredentials([sshUserPrivateKey(credentialsId: 'deploy-ssh-key', keyFileVariable: 'SSH_KEY')]) {
                        // Check if service is running and diagnose port binding
                        sh """
                            ssh -i ${SSH_KEY} -o StrictHostKeyChecking=no ${APP_SERVER_USER}@${APP_SERVER_HOST} << 'EOF'
                                echo '=== Service Status ==='
                                sudo systemctl is-active --quiet ${APP_NAME}
                                if [ \$? -eq 0 ]; then
                                    echo 'Service ${APP_NAME} is running'
                                else
                                    echo 'Service ${APP_NAME} is not running'
                                    sudo systemctl status ${APP_NAME}
                                    echo '=== Application Logs (last 200) ==='
                                    sudo journalctl -u ${APP_NAME} --no-pager -n 200 || true
                                    echo '=== Unit Definition (systemctl cat) ==='
                                    sudo systemctl cat ${APP_NAME} || true
                                    echo '=== Effective Environment (from unit drop-in) ==='
                                    sudo sed -n '1,200p' /etc/systemd/system/${APP_NAME}.service.d/override.conf || true
                                    echo '=== Appsettings on server (first 200 lines) ==='
                                    sudo sed -n '1,200p' ${CURRENT_PATH}/appsettings.json || true
                                    exit 1
                                fi
                                
                                echo '=== Port Binding Check ==='
                                sudo netstat -tlnp | grep :${APP_PORT} || echo 'No process listening on port ${APP_PORT}'
                                
                                echo '=== Check if binding to 0.0.0.0 ==='
                                sudo netstat -tlnp | grep :${APP_PORT} | grep 0.0.0.0 || echo 'Application not binding to 0.0.0.0'
                                
                                echo '=== Application Logs ==='
                                sudo journalctl -u ${APP_NAME} --no-pager -n 100
                                echo '=== Unit Definition (systemctl cat) ==='
                                sudo systemctl cat ${APP_NAME} || true
                                echo '=== Effective Environment (from unit drop-in) ==='
                                sudo sed -n '1,120p' /etc/systemd/system/${APP_NAME}.service.d/override.conf || true
                                echo '=== Appsettings on server (first 200 lines) ==='
                                sudo sed -n '1,200p' ${CURRENT_PATH}/appsettings.json || true
EOF
                        """

                        // Test HTTPS endpoint from external (via domain)
                        sh """
                            echo "Testing external HTTPS connection to https://${APP_DOMAIN}..."
                            curl -f https://${APP_DOMAIN} || {
                                echo "External HTTPS health check failed - application not responding on https://${APP_DOMAIN}"
                                echo "This might be a Cloudflare tunnel or binding issue"
                                exit 1
                            }
                        """

                        echo "Health check passed - application is running successfully!"
                    }
                }
            }
        }
    }
    
    post {
        always {
            echo "Pipeline execution completed"
            cleanWs()
        }
        success {
            echo "Pipeline completed successfully!"
        }
        failure {
            echo "Pipeline failed. Check logs for more details."
        }
    }
}
