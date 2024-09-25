var products = null;
var productsContainer = document.getElementById("product-tabel-container");
var warningMessage = document.getElementById("warning-msg");
var tabelBody = document.getElementById("tabel-body");

function handelREnderData() {
  if (products && products.length !== 0) {
    console.log("Products are available");
    productsContainer.classList.remove("d-none");
    productsContainer.classList.add("d-block");
    warningMessage.classList.add("d-none");
    warningMessage.classList.remove("d-block");

    var rows_eleemnts = "";

    for (var i = 0; i < products.length; i++) {
      rows_eleemnts += `
             <tr>
            <th>${i + 1}</th>
            <td>${products[i].name}</td>
            <td>${products[i].cat}</td>
            <td>${products[i].price}</td>
            <td>
            ${products[i].dec}
            </td>
            <td>
              <button class="btn btn-outline-success">
                <i class="fa-solid fa-pen-to-square"></i>
              </button>
            </td>
            <td>
              <button class="btn btn-outline-danger">
                <i class="fa-solid fa-trash"></i>
              </button>
            </td>
          </tr>
          
        `;
    }

    tabelBody.innerHTML = rows_eleemnts;
  } else {
    warningMessage.classList.remove("d-none");
    warningMessage.classList.add("d-block");
    productsContainer.classList.add("d-none");
    productsContainer.classList.remove("d-block");
  }
}
handelREnderData();
//Createion Opration

//1- get input elements
var productName = document.getElementById("product_name");
var productCat = document.getElementById("product_category");
var productPrice = document.getElementById("product_price");
var productDesc = document.getElementById("prodct_desc");
var createBtn = document.getElementById("create-btn");
var productForm = document.getElementById("product-form");

//2- read values
productForm.onsubmit = function (event) {
  event.preventDefault();
  if (!products) {
    products = [];
  }

  var product = {
    name: productName.value,
    cat: productCat.value,
    price: productPrice.value,
    dec: productDesc.value,
  };

  products.push(product);
  console.log();
  handelREnderData();
  //   console.log(productName.value);
  //   console.log();
  //   console.log(productPrice.value);
  //   console.log(productDesc.value);
};

//3- when user click add produ btn
var arr = [];
console.log(typeof arr);
