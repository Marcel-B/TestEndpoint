@NonCPS
def showChangeLogs() {
 def changeLogSets = currentBuild.changeSets
 def foo = ""
 for (int i = 0; i < changeLogSets.size(); i++) {
     def entries = changeLogSets[i].items
     for (int j = 0; j < entries.length; j++) {
         def entry = entries[j]
         foo = foo + "${new Date(entry.timestamp)}: ${entry.author}:  ${entry.msg}"
         foo = foo + '<BR>'
         def files = new ArrayList(entry.affectedFiles)
             for (int k = 0; k < files.size(); k++) {
                 def file = files[k]
                 foo = foo + " - ${file.editType.name} ${file.path}"
                 foo = foo + '<BR>'
             }
         foo = foo + '<BR>'
     }
 }
 return foo
}

def sendMail(String mssg){
 emailext (
     subject: "STARTED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'",
     body: """<p>STARTED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
     <p>${mssg}</p><p>Check console output at &QUOT;<a href='${env.BUILD_URL}'>${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>&QUOT;</p>""",
     to: "marcel.benders@outlook.de")
}


node ('marcelbenders.de') {
 def mvnHome
 def commitId

 if(env.BRANCH_NAME != 'master' && env.BRANCH_NAME != 'dev' )
     return 
 stage('preparation') { 
     cleanWs()
     checkout scm
     commitId = sh(returnStdout: true, script: 'git rev-parse HEAD')
 }

 try{
     stage('restore') {
         sh 'dotnet restore --configfile TestPoint/NuGet.config' 
     }
 }catch(Exception ex){
     sendMail("RESULT: ${currentBuild.result}")
     currentBuild.result = 'FAILURE'
     sendMail("RESULT: ${currentBuild.result}")
     echo "RESULT: ${currentBuild.result}"
     return 
 }
 
 try{
     stage('build'){
         sh 'dotnet build'
     }
 }catch(Exception ex){
     currentBuild.result = 'FAILURE'
     sendMail("RESULT: ${currentBuild.result}")
     echo "RESULT: ${currentBuild.result}"
     return
 }

 try{
     stage('publish'){
         sh 'dotnet publish -c Release'
     }
 }catch(Exception ex){
     currentBuild.result = 'FAILURE'
     sendMail("RESULT: ${currentBuild.result}")
     echo "RESULT: ${currentBuild.result}"
     return
 }

 try{
     stage('test') {
     }
 }catch(Exception ex){
     currentBuild.result = 'FAILURE'
     sendMail("RESULT: ${currentBuild.result}")
     echo "RESULT: ${currentBuild.result}"
     return
 }

 try{
     if(env.BRANCH_NAME == 'master'){
	 withCredentials([string(credentialsId: 'NexusNuGetToken', variable: 'token')]) {
         stage('containerize'){
            mvnHome = env.BUILD_NUMBER
            sh "docker build --build-arg var_name=${token} -t docker.qaybe.de/testpoint:1.0.${mvnHome} ."
            withDockerRegistry(credentialsId: 'DockerRegistry', toolName: 'QaybeDocker', url: 'https://docker.qaybe.de') {
                 sh "docker push docker.qaybe.de/testpoint:1.0.${mvnHome}"
            }
		}
        }
     }   
 }catch(Exception ex){
     currentBuild.result = 'FAILURE'
     sendMail("RESULT: ${currentBuild.result}")
     echo "RESULT: ${currentBuild.result}"
     return
 }

 try{
     stage('clean'){
         cleanWs()
     }
 }catch(Exception ex){
     currentBuild.result = 'FAILURE'
     sendMail("RESULT: ${currentBuild.result}")
     echo "RESULT: ${currentBuild.result}"
     return
 }      

 if(env.BRANCH_NAME == 'master')
     stage('notify'){
         def mailText = showChangeLogs()
         sendMail(mailText)
    }
}
