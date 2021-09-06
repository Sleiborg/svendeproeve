function checkAuth(){
        var token = getCookie("access_token")

    var isSigninELM = document.querySelector(".issignin")

    // if(!token){
    //     window.location.href = "/HTML/Index.html"
    // }

    

    isSigninELM.style.visibility = "hidden"
}

checkAuth()










