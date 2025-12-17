pipeline {
    agent any
    
    environment {
        // GitHub repository configuration
        GITHUB_REPO = 'https://github.com/HOGENT-RISE/dotnet-2526-tiao2.git'
        GITHUB_USERNAME = 'badramr1'

        // Email notification configuration
        EMAIL_RECIPIENTS = 'tiaopipeline@gmail.com,badr.amri@student.hogent.be,lars.devos@student.hogent.be,brent.lissens@student.hogent.be,jonathan.laekeman@student.hogent.be,jens.vanhoeylandt@student.hogent.be,iliass.assoued@student.hogent.be,ali.aydinlioglu@student.hogent.be,wim.dedulle@student.hogent.be,pieter.pletinckx@student.hogent.be,pieter.swillens@student.hogent.be,andy.wauters@student.hogent.be,marek.zakrzewski@student.hogent.be'
        
        // Application server configuration
        // Default value; will be auto-resolved from ops inventory if available
        APP_SERVER_HOST = '192.168.6.11'
        APP_SERVER_USER = 'deploy'
        APP_NAME = 'Rise.Server'
        APP_PORT = '5001'
        
        // Domain configuration for HTTPS (Cloudflare)
        APP_DOMAIN = 'campus.badrlab.xyz'
        
        // Database configuration
        DB_SERVER = '192.168.6.12'
        
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
                        branches: [[name: '*/ops/main2']],
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
                    
                        // Prepare published config files before transfer
                        sh """
                            # Backend config - use internal IP for server-to-server communication
                            if ls ${PUBLISH_DIR}/appsettings*.json >/dev/null 2>&1; then
                                sed -i 's|https\\?://\\(localhost\\|127\\.0\\.0\\.1\\|0\\.0\\.0\\.0\\)\\(:[0-9]\\+\\)\\?|http://${APP_SERVER_HOST}|g' ${PUBLISH_DIR}/appsettings*.json
                                perl -i -pe 's/"DatabaseConnection"\\s*:\\s*"[^"]*"/"DatabaseConnection": "server=${DB_SERVER};port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none"/' ${PUBLISH_DIR}/appsettings.json
                            fi
                            
                            # Frontend config - use public domain for browser requests
                            if ls ${PUBLISH_DIR}/wwwroot/appsettings*.json >/dev/null 2>&1; then
                                sed -i 's|https\\?://\\(localhost\\|127\\.0\\.0\\.1\\|0\\.0\\.0\\.0\\|${APP_SERVER_HOST}\\)\\(:[0-9]\\+\\)\\?|https://${APP_DOMAIN}|g' ${PUBLISH_DIR}/wwwroot/appsettings*.json
                            fi
                        """
                        
                        // Create release directory on remote server with proper permissions
                        sh """
                            ssh  -i \${SSH_KEY} -o StrictHostKeyChecking=no ${APP_SERVER_USER}@${APP_SERVER_HOST} "sudo mkdir -p ${releaseDir} && sudo chown ${APP_SERVER_USER}:${APP_SERVER_USER} ${releaseDir} && sudo chmod 755 ${releaseDir}"
                        """
                        
                        // Transfer files directly using rsync
                        sh """
                            # Use rsync to transfer files directly (more reliable than tarball)
                            rsync -avz --delete -e "ssh  -i \${SSH_KEY} -o StrictHostKeyChecking=no" ${PUBLISH_DIR}/ ${APP_SERVER_USER}@${APP_SERVER_HOST}:${releaseDir}/
                            
                            # Fix ownership on remote server
                            ssh  -i \${SSH_KEY} -o StrictHostKeyChecking=no ${APP_SERVER_USER}@${APP_SERVER_HOST} "sudo chown -R ${APP_SERVER_USER}:${APP_SERVER_USER} ${releaseDir}"
                        """
                    
                        // Fix permissions and deploy application
                        sh """
                            ssh  -i \${SSH_KEY} -o StrictHostKeyChecking=no ${APP_SERVER_USER}@${APP_SERVER_HOST} << 'EOFMAIN'
                                sudo chown -R ${APP_SERVER_USER}:${APP_SERVER_USER} ${releaseDir}
                                sudo chmod -R 755 ${releaseDir}
                                sudo rm -rf ${CURRENT_PATH}
                                sudo mkdir -p ${CURRENT_PATH}
                                sudo cp -r ${releaseDir}/* ${CURRENT_PATH}/
                                sudo chown -R ${APP_SERVER_USER}:${APP_SERVER_USER} ${CURRENT_PATH}
                                sudo chmod -R 755 ${CURRENT_PATH}
                                
                                sudo mkdir -p /etc/systemd/system/${APP_NAME}.service.d
                                cat << 'EOFSERVICE' | sudo tee /etc/systemd/system/${APP_NAME}.service.d/override.conf >/dev/null
[Service]
Environment=ASPNETCORE_ENVIRONMENT=Development
Environment=ASPNETCORE_URLS=http://0.0.0.0:${APP_PORT}
Environment=APP_SERVER_HOST=${APP_SERVER_HOST}
Environment=ConnectionStrings__DatabaseConnection=server=${DB_SERVER};port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none
Environment=DatabaseConnection=server=${DB_SERVER};port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none
Environment=FrontendUrl=https://${APP_DOMAIN}
WorkingDirectory=${CURRENT_PATH}
Restart=always
RestartSec=5
EOFSERVICE
                                
                                # Backend config updates on server
                                if ls ${CURRENT_PATH}/appsettings*.json >/dev/null 2>&1; then
                                    sudo sed -i 's|https\\?://localhost\\(:[0-9]\\+\\)\\?|http://${APP_SERVER_HOST}|g' ${CURRENT_PATH}/appsettings*.json
                                    sudo sed -i 's|https\\?://127\\.0\\.0\\.1\\(:[0-9]\\+\\)\\?|http://${APP_SERVER_HOST}|g' ${CURRENT_PATH}/appsettings*.json
                                    sudo sed -i 's|https\\?://0\\.0\\.0\\.0\\(:[0-9]\\+\\)\\?|http://${APP_SERVER_HOST}|g' ${CURRENT_PATH}/appsettings*.json
                                    sudo perl -i -pe 's/"DatabaseConnection"\\s*:\\s*"[^"]*"/"DatabaseConnection": "server=${DB_SERVER};port=3306;database=campusappdb;user=admin;password=admin123;SslMode=none"/' ${CURRENT_PATH}/appsettings.json
                                fi

                                # Frontend config updates on server - use public domain
                                if ls ${CURRENT_PATH}/wwwroot/appsettings*.json >/dev/null 2>&1; then
                                    sudo sed -i 's|https\\?://localhost\\(:[0-9]\\+\\)\\?|https://${APP_DOMAIN}|g' ${CURRENT_PATH}/wwwroot/appsettings*.json
                                    sudo sed -i 's|https\\?://127\\.0\\.0\\.1\\(:[0-9]\\+\\)\\?|https://${APP_DOMAIN}|g' ${CURRENT_PATH}/wwwroot/appsettings*.json
                                    sudo sed -i 's|https\\?://0\\.0\\.0\\.0\\(:[0-9]\\+\\)\\?|https://${APP_DOMAIN}|g' ${CURRENT_PATH}/wwwroot/appsettings*.json
                                    sudo sed -i 's|https\\?://${APP_SERVER_HOST}\\(:[0-9]\\+\\)\\?|https://${APP_DOMAIN}|g' ${CURRENT_PATH}/wwwroot/appsettings*.json
                                fi
                                
                                sudo systemctl daemon-reload
                                sudo systemctl stop ${APP_NAME} || true
                                sudo systemctl start ${APP_NAME}
                                sudo systemctl enable ${APP_NAME}
                                
                                echo '=== Deployment completed ==='
                                sudo systemctl is-active --quiet ${APP_NAME} && echo 'Service is running!' || echo 'Service failed to start'
EOFMAIN
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
                        // Check if service is running
                        sh """
                            ssh  -i \${SSH_KEY} -o StrictHostKeyChecking=no ${APP_SERVER_USER}@${APP_SERVER_HOST} << EOF
                                if ! sudo systemctl is-active --quiet ${APP_NAME}; then
                                    echo 'Service ${APP_NAME} is not running'
                                    sudo systemctl status ${APP_NAME}
                                    sudo journalctl -u ${APP_NAME} --no-pager -n 100
                                    exit 1
                                fi
                                echo 'Service ${APP_NAME} is running'
                                sudo netstat -tlnp | grep :${APP_PORT}
                                sudo journalctl -u ${APP_NAME} --no-pager -n 50
EOF
                        """
                    }
                }
            }
        }
    }
    
  post {
    success {
        echo "✅ Build succeeded!"
        
        mail to: "${EMAIL_RECIPIENTS}",
             subject: "✅ Build Success: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
             body: "The build was successful.\nJob: ${env.JOB_NAME}\nBuild: #${env.BUILD_NUMBER}\nURL: ${env.BUILD_URL}/console"
    }

    failure {
        echo "❌ Build failed!"

        mail to: "${EMAIL_RECIPIENTS}",
             subject: "❌ Build FAILED: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
             body: "The build has failed.\nJob: ${env.JOB_NAME}\nBuild: #${env.BUILD_NUMBER}\nURL: ${env.BUILD_URL}/console\nCheck logs in Jenkins."
    }

    always {
        echo "Pipeline finished."
    }
}
}
