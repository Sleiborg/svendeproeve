function checkAuth(){
        var token = getCookie("access_token")

    var isSigninELMs = document.querySelectorAll(".issignin")
    for(i = 0; i < isSigninELMs.length; i++){

        if(!token){
            isSigninELMs[i].style.visibility = "hidden"
        }    
    }
}
checkAuth()










