var conStr = "localhost:5000";
var dynImage = "/media/images/";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://" + conStr + "/products")
    .configureLogging(signalR.LogLevel.Information)
    .build();

    connection.on("ReceiveProducts", function (product){

        const html = `
        <div class="card-panel product white row" data-id="${product.productId}">
            <div class="product-details">
            <div class="product-title">${product.title}</div>
            <div class="product-description">${product.descriptions}</div>
            <div class="product-price">${product.price}</div>
            <img class="img-size"alt="product thumb" ${product.image}>
            </div>
  
            <button class="add" >Tilføj</button>

            <div class="product-delete issignin">
                <i class="material-icons delete-product" data-id="${product.productId}">delete_outline</i>
            </div>
        </div>`
        $(".products").append(html)  
    })

    function absolutePath(href)
    // var absolutePath = function(href) 
    {
        var link = document.createElement("a");
        link.href = href;
        return link.href;
    }

async function start(){
    try{
        await connection.start();
        console.log("SignalR Connected.");
    }catch (err){
        console.log(err);
        setTimeout(start,5000);
    }
};

connection.onclose(start);
// Start the connection.
start();


function getProducts(func){
    // API GET
    const productsElm = document.querySelector('.products');
    //alert('Test')
    $.ajax({
        url:"http://" + conStr + "/api/product",
        type: "Get",
        Data:$(this).serialize(),
        dataType: "json",
        success: function(data){
            for(let index = 0; index < data.length; index++){
                const element = data[index];
                const html = `
                <div class="card-panel product white row" data-id="${element.productId}">
                    <div class="product-details">
                    <div class="product-title">${element.title}</div>
                    <div class="product-description">${element.descriptions}</div>
                    <div class="product-price">${element.price}</div>
                    <img class="img-size" src="http://${conStr + dynImage}${element.image}" alt="product thumb">
                </div>

                    <button class="add" >Tilføj</button>
                    <button class="remove" >Fjern fra kurv</button>

                    <div class="product-delete issignin">
                        <i class="material-icons delete-product" data-id="${element.productId}">delete_outline</i>
                    </div>
                </div>
                `
                productsElm.innerHTML += html
            }

            var product = {
                title: 'Some title.',
                descriptions: 'Some descriptions.'
            }

             // Create new product
             $('#createForm').submit(function(e){
                 console.log("a new product created")
                 $.ajax({
                     url:"http://" + conStr + "/api/product/create",
                     type: "post",
                     data:$(this).serialize(),
                     success: function(data){
                         //alert(data.title);
                     },error:function(xhr){
                         console.error("Fejl");
                     }
                 })
                 e.preventDefault();
             })

             //API Delete
             $(".delete-product").click(function(){
                 var id=$(this).attr("data-id");
                 $.ajax({
                     url:"http://" + conStr + "/api/product/delete/" + id,
                     type:"Delete",
                     contentType:'application/json',
                     success: function(){
                        console.log("Slettet");
                        $(`.product[data-id='${id}']`).remove()
                     }, error: function(xhr){
                        console.error("bla bla bla");
                     }
                 })
             })
             func()
        }
    })
}


