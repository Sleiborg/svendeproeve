
// Basket localstoarge name
let basketname='basket'

// Set basket localstoarge
function setbasket(data){
    localStorage[basketname] = data
}

// Get basket localstoarge 
function getBasket(){
    return localStorage[basketname]
}
 
//Get all products from basket 
function getItems() {
    var json_basket = getBasket()
    let basket
    if(json_basket){
        basket = JSON.parse(json_basket)
        return basket
    }
}

// Remove a item from basket
function removeItem(item){
    let items = this.getItem()
    for(let i = 0; i < items.lenght; i++)
        if(items[i].productId ==  item.productId){
            items.splice(i, 1)
            setbasket(JSON.stringify(items))
            break
        }
}

// Get a product from basket by product id
function getItem(productId) {
    var json_basket = getBasket()        
    let basket
    if(json_basket){
        basket = JSON.parse(json_basket)
        for(let b_item of basket)
            if(b_item.productId == productId)
                return b_item
    }
}

// Check for is a item exists in basket
function itemExsites(productId){
    return !!this.getItem(productId)
}

// Set/edit a item in basket
function setItem(item) 
{    
    let items = getItems();    
    for(let i = 0; i < items.length;i++) 
    {              
        if(items[i].productId == item.productId) 
        {
            items[i] = item
            break
        }
    }    
    setbasket(JSON.stringify(items))   
}

// Add new item to basket
function addItem(item){
    
    var json_basket = getBasket()
    let basket
    if(json_basket)
        basket = JSON.parse(json_basket)
    else
        basket = []

    basket.push(item) 
    setbasket(JSON.stringify(basket));             	
}

