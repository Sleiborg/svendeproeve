const products = document.querySelector('.products');

document.addEventListener('DOMContentLoaded', function(){
    //Nav menu
    const menus = document.querySelectorAll('.side-menu');
    M.Sidenav.init(menus, {edge: 'right'});

    //Add product form
    const forms = document.querySelectorAll('.side-form');
    M.Sidenav.init(forms, {edge: 'left'});
});

//Render products data
    const renderProduct = (data, id) =>{
        const html =
        `
        <div class="card-panel product white row" data-id="${id}">
            <div class="product-details">
                <div class="product-title">${data.title}</div>
                <div class="product-description">${data.descriptions}</div>
                <img src"/img/No_Foto.png" alt="product thumb">
            </div>

            <div class="product-delete">
                <i class="material-icons" data-id"${id}">delete_outline</id>
            </div>
        </div>
        `;

        products.innerHTML += html
    };