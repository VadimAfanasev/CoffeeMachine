pipeline { 
  agent { label 'tap-agents' } 
  options { 
    timestamps() 
    timeout(time: 1, unit: 'HOURS') 
  } 
  environment { 
    PROJECT_NAME = 'CoffeeMachine' //Вставить название проекта 
    NAME = 'registry.tomskasu.ru/gs-platform-team/gs-templates/CoffeeMachine' //Вставить путь до реестра проекта 
    TAG = 'latest' 
  } 
  stages { 
    stage('Build') { // уточнить
      agent { 
        docker { 
          image 'registry.tomskasu.ru/devops/dockerify/dotnetsdk:7.0-focal'  // уточнить
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
        withSonarQubeEnv('sonar.tomskasu.ru') { 
          sh(script: 'dotnet sonarscanner begin /k:$PROJECT_NAME /version:$BUILD_NUMBER', 
            label: 'begin SonarQube scan') 
          sh(script: 'dotnet restore CoffeMachine/CoffeMachine.sln', 
            label: 'Restore') 
          sh(script: 'dotnet build CoffeMachine/CoffeMachine.sln --configuration Release --no-restore', 
            label: 'build app') 
          sh(script: 'dotnet sonarscanner end', 
            label: 'end SoanrQube scan') 
        } 
        sh(script: 'dotnet publish CoffeMachine/CoffeMachine.sln --configuration Release --output app', 
            label: 'publish app') 
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
