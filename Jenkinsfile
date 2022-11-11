#!groovy
//  groovy Jenkinsfile
properties([disableConcurrentBuilds()])

pipeline  {
    
    agent { 
        label 'master'
        }
    options {
        buildDiscarder(logRotator(numToKeepStr: '10', artifactNumToKeepStr: '10'))
        timestamps()
    }
    stages {
		stage("Removing old containers") {
            steps {
                echo 'Removing containers ...'
                 dir('.'){
                   sh ' docker ps -q --filter "name=dashboard_backend" | grep -q . && docker stop dashboard_backend || echo Not Found'
				
                    sh 'docker ps -q --filter "name=dashboard_backend" | grep -q . && docker rm dashboard_backend || echo Not Found'
                }
            }
        }
        stage("Removing old images") {
            steps {
                echo 'Removing images ...'
                 dir('.'){
                    sh 'docker ps -q --filter "name=movchanets/dashboard_backend" | grep -q . && docker rmi movchanets/dashboard_backend || echo Not Found'

                }
            }
        }
        stage("Creating images") {
            steps {
                echo 'Creating docker image ...'
                    dir('.'){
                    sh "docker build -t movchanets/dashboard_backend ."
                }
            }
        }
        stage("docker login") {
            steps {
                echo " ============== docker login =================="
                withCredentials([usernamePassword(credentialsId: 'DockerHub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) {
                    sh '''
                    docker login -u $USERNAME -p $PASSWORD
                    '''
                }
            }
        }
        stage("docker push image") {
            steps {
                echo " ============== pushing image =================="
                sh '''
                docker push movchanets/dashboard_backend:latest
                '''
            }
        }
        
        stage("docker run") {
            steps {
                echo " ============== starting frontend =================="
                sh '''
                docker run -d --restart=always --name dashboard_backend -p 4444:8080 movchanets/dashboard_backend:latest
                '''
            }
        }
    }
}
