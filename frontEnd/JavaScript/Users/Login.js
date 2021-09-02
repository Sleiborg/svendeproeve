$('#login').submit(function(e){

    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;

    $.ajax({
        type:'POST',
        url:'https://localhost:44373/api/Identity/signin',
        contentType:'application/json',
        data: JSON.stringify({email: email, password:password}),
        dataType:'JSON',
        success: function(data){
            document.cookie = `access_token=${data.accessToken}; path=/`;
            window.location.href = "/HTML/Index.html";
        },
        error: function(error){
            alert(error.message)
        }
    })
    e.preventDefault()
})


