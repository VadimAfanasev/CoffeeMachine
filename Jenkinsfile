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
	stage('Initialize'){
            steps {
                 script {
		     def dockerHome = tool 'myDocker'
                     env.PATH = "${dockerHome}/bin:${env.PATH}"
                 }
            }
        }
        stage('Build') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/aspnet:7.0'
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
		  sh(script: 'dotnet publish src/CoffeMachine.sln--configuration Release --output app',
                  label: 'publish app')
                  sh(script: 'chmod -R 777 app/',
                  label: 'changed rules on app directory')
      	    }
        }
    }
}