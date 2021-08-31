$('#login').submit(function(e){
    $.ajax({
        type:'POST',
        url:'https://localhost:5001/api/Identity/signin',
        contentType:'application/json',
        data: JSON.stringify({email: 'Admin@eg.dk', password:'Admin123!'}),
        dataType:'JSON',
        success: function(data){
            document.cookie = `access_token=${data.accessToken}; path=/`;
        },
        error: function(error){
            alert(error.message)
        }
    })
    e.preventDefault()
})