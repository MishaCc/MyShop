

let settings = document.getElementById('settings');
let menu = document.getElementById('menu');
let exit = document.getElementById('exit');
let logins = document.getElementsByClassName('login');
let loginForm = document.getElementById('login-form-wrap');
let forms = document.getElementsByClassName('form');
let formExit = document.getElementsByClassName('form-exit');
let en = document.getElementById('enter');
let register = document.getElementById('register');
let body = document.getElementById('body');
let head = document.getElementById('ge');
let show = document.getElementById('show');
let  password = document.getElementById('password');
let showReg = document.getElementById('show-reg');
let registPassword = document.getElementById('regist-password');
let help = document.getElementById('login-regist');
let text = document.getElementById("test");
let buttonGoods = document.getElementsByClassName('goods-add');
let buttonsProducts =  document.getElementsByClassName('BasketProduct');
let showRequest = document.getElementById('writeRequest');
let requestForm =document.getElementById('requestForm');
let exitRf = document.getElementById('exitRf');
let exitCp = document.getElementById('exitCp');
let сomplainForm = document.getElementById('сomplainForm');
let showComplain =  document.getElementById('showComplain');
let card = document.getElementById('card');
 let unitBlock = document.getElementById('unitC');
 let CardRadio = document.getElementById('Card');
 let CashRadio = document.getElementById('Cash');
 let loginP = document.getElementById('showLogin');
 let logP = document.getElementById('password');
 window.addEventListener('click',function(event){
	 if(event.target.id==loginP.id){
	   logP.setAttribute('type', 'text');
	 }else {
        logP.setAttribute('type', 'password');
    }
    
 });
window.addEventListener('click',function(event){
	 if(event.target.id==CashRadio.id){
  	   card.style.display ='none';
	 }
	  if(event.target.id==CardRadio.id){
  	   card.style.display ='block';
	 }
	
 });
window.addEventListener('click',function(event){
	if(event.target.id==showRequest.id){
		requestForm.style.display='block';
	}
	if(event.target.id==exitRf.id){
		requestForm.style.display='none';
	}

	if(event.target.id==exitCp.id){
		сomplainForm.style.display='none';
	}
});
 let completed = document.getElementsByClassName('Completed');
 function ComletedOrders(id){
 $.ajax({
        type: 'POST',
        url: '/Home/ToComplete',
        data: {ProductId:id},
        });
  	for(let i=0;i<completed.length;i++){
		 if(id==completed[i].id){
			completed[i].style.display='none';
			completed[i].after('Помічено як виконане');
			}
		}
}
function Complain(id){
	$(UserIdCompain).val(id);
	сomplainForm.style.display='block';
}
function DeleteProductFromBasket(id){
     let result = confirm("Ви дісйно хочетe видалити цей продукт?");
	 if(result){
	for(let i=0;i<buttonsProducts.length;i++){
		if(id==buttonsProducts[i].id){
			buttonsProducts[i].style.display='none';
		}
	}
		$.ajax({
        type: 'POST',
        url: '/Home/DeleteProductFromBaket',
        data: {productId:id},
        });
	 }
}
function AddProduct(productId, userId) {
	
    $.ajax({
        type: 'POST',
        url: '/Home/AddProduct',
        data: { UserId: userId, ProductId: productId }
    });
	 for(let i=0;i<buttonGoods.length;i++){
		if(productId==buttonGoods[i].id){
			buttonGoods[i].style.display='none';
		}
	}
}
function RedirectToBasket(id) {
    $.ajax({
        type: 'POST',
        url: location.href =`/Home/Basket?userId=${id}`
    });
}
window.addEventListener('click', function (event) {
    for (let i = 0; i < buttonGoods.length; i++) {
        if (event.target.id === buttonGoods[i].id) {
       
        }
    }
});
window.addEventListener('click', function (event) {
    if (event.target.id === menu.id || event.target.classList.contains('menu-line')) {
        settings.style.animationName = 'slidein';
        settings.style.animationDuration ='0.5s';
        settings.style.transform ='translateX(0px)';
    }
    if(event.target.id===exit.id){
        settings.style.animationName = 'slidein2';
        settings.style.animationDuration ='0.5s';
        settings.style.transform ='translateX(-345px)';
    }
});
function GetId(id) {
    $('#pageId').val(id);
        $.ajax({
            type: 'POST',
        });
}


let refBasket = document.getElementById('ref-basket');
let a=0;

let product = document.getElementById('product');
let characteristic = document.getElementById('more');
let reviews = document.getElementById('reviews');
let productWrap = document.getElementById('product-wrap');
let characteristicsBlock = document.getElementById('characteristics-block');
let productMenus = document.getElementsByClassName('product-menu');
let reviewBlock = document.getElementById('review');
characteristic.addEventListener('click',function (){
    productWrap.style.display ='none';
    reviewBlock.style.display = 'none';
    characteristicsBlock.style.display='flex';
});
product .addEventListener('click',function (){
    productWrap.style.display ='block';
    reviewBlock.style.display = 'none';
    characteristicsBlock.style.display='none';
});
reviews.addEventListener('click',function (){
    productWrap.style.display ='none';
    characteristicsBlock.style.display='none';
    reviewBlock.style.display = 'block';

});

