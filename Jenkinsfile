pipeline {
    agent any
    
    environment {
        // GitHub repository configuration
        GITHUB_REPO = 'https://github.com/HOGENT-RISE/dotnet-2526-tiao2.git'
        GITHUB_USERNAME = 'badramr1'
        
        // Application server configuration
        APP_SERVER_HOST = '192.168.56.12'
        APP_SERVER_USER = 'deploy'
        APP_NAME = 'Rise.Server'
        APP_PORT = '5000'
        
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
                        branches: [[name: '*/main']],
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
                                    exit 1
                                fi
                                
                                echo '=== Port Binding Check ==='
                                sudo netstat -tlnp | grep :${APP_PORT} || echo 'No process listening on port ${APP_PORT}'
                                
                                echo '=== Check if binding to 0.0.0.0 ==='
                                sudo netstat -tlnp | grep :${APP_PORT} | grep 0.0.0.0 || echo 'Application not binding to 0.0.0.0'
                                
                                echo '=== Application Logs ==='
                                sudo journalctl -u ${APP_NAME} --no-pager -n 10
                                
                                echo '=== Local Connection Test ==='
                                curl -f http://localhost:${APP_PORT} || echo 'Local connection failed'
EOF
                        """

                        // Test HTTP endpoint from external
                        sh """
                            echo "Testing external connection to ${APP_SERVER_HOST}:${APP_PORT}..."
                            curl -f http://${APP_SERVER_HOST}:${APP_PORT} || {
                                echo "External health check failed - application not responding on port ${APP_PORT}"
                                echo "This might be a firewall or binding issue"
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