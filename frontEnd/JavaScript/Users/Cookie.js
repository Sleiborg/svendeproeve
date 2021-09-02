function removeCookie(key){
    document.cookie = `${key}=;expires=01-01-0001 00:00:01`
}

function getCookie(key){
    var cookies = document.cookie.split(';')
    var cookie
    for(var i=0; i < cookies.length; i++){
        cookie = cookies[i].split('=')
        if(cookie[0].trim()==key)
            return cookie[1].trim()
    }
}

function setCookie(key, value, expires, path){
    document.cookie = `${key}=${value};expires=${expires};path=${path}`
}