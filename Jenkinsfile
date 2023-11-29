pipeline {
    agent any
    options {
        timestamps()
        timeout(time: 1, unit: 'HOURS')
    } 
    environment {
        PROJECT_NAME = 'Coffee_Machine'
        NAME = 'Coffee_Machine'
        TAG = 'latest'
    }
    stages {
        stage('check out scm') {
            steps {
                checkout scm
            }
        }
        stage('Build and Test') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:7.0'
                    args "-v ${PWD}:/usr/src/app -w /usr/src/app -u root --privileged -e PATH='/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/root/.dotnet/tools'"
                    reuseNode true
                    label 'build-image'
                }
            }
            environment {
                ASPNETCORE_ENVIRONMENT = 'Production'
                DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
                DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
            }
			steps {
				sh(script: 'dotnet test CoffeMachine/CoffeeMachine.Tests/CoffeeMachine.Tests.csproj -c Release --output app',
					label: 'test app')
				sh(script: 'chmod -R 777 app/',
					label: 'changed rules on app directory')
				}
        } 
		stage('Docker image create and push') {
			when {
				branch 'main'
			}
			steps {
				script {
					def BackImage = docker.build("${env:NAME}:${env:TAG}")
					BackImage.push()

					BackImage.push("${env:BUILD_NUMBER}")
				}
			}
		}
    } 
	post {
		always {
		  cleanWs(cleanWhenNotBuilt: false,
			deleteDirs: true,
			disableDeferredWipeout: true,
			notFailBuild: true,
			patterns: [[pattern: '.gitignore', type: 'INCLUDE'],
			  [pattern: '.propsfile', type: 'EXCLUDE'],
			  [pattern: 'bin', type: 'INCLUDE'],
			  [pattern: 'app', type: 'INCLUDE']])
		}	
	}
}