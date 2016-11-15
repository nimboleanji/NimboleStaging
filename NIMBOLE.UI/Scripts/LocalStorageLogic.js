(function () {
    //The Product Object used to store data in the LocalStorage 
    var Product = {
        ProductType: "",
        Code: 0,
        Name: "",
        Price: 0,
        Quntity: 0
    };
    //JavaScript object containing methods for LocalStorage management 
    var applogic = {
        
        //Clear All Entries, by reading all elements having class as c1 
        clearuielements: function () {
            
            var inputs = document.getElementsByClassName("c1");
            for (i = 0; i < inputs.length; i++) {
                inputs[i].value = "";
            }
        },
        //Save Entry in the Localstorage by eading values entered in the 
        //UI 
        saveitem: function () {
            
            var lscount = localStorage.length; //Get the Length of the LocalStorage
            //Read all elements on UI using class name 

            var inputs = document.getElementsByClassName("c1");
            Product.ProductType = inputs[0].value;
            Product.Code = inputs[1].value;
            Product.Name = inputs[2].value;
            Product.Price = inputs[3].value;
            Product.Quntity = inputs[4].value;

            //Convert the object into JSON ans store it in LocalStorage 
            localStorage.setItem("Product_" + lscount, JSON.stringify(Product));
            //Reload the Page 
            location.reload();
        },
        //Method to Read Data from the local Storage 
        loaddata: function () {
            var datacount = localStorage.length;
            if (datacount > 0) {
                var render = "<table border='1'>";
                render += "<tr><th>ProductType</th><th>Code</th><th>Name</th><th>Price</th><th>Quntity</th></tr>";
                for (i = 0; i < datacount; i++) {
                    var key = localStorage.key(i); //Get  the Key 
                    var product = localStorage.getItem(key); //Get Data from Key 
                    var data = JSON.parse(product); //Parse the Data back into the object 

                    render += "<tr><td>" + data.ProductType + "</td><td>" + data.Code + " </td>";
                    render += "<td>" + data.Name + "</td>";
                    render += "<td>" + data.Price + "</td>";
                    render += "<td>" + data.Quntity + "</td></tr>";
                }
                render += "</table>";
                dvcontainer.innerHTML = render;
            }
        },
        //Method to Clear Storage 
        clearstorage: function () {
            var storagecount = localStorage.length; //Get the Storage Count 
            if (storagecount > 0) {
                for (i = 0; i < storagecount; i++) {
                    localStorage.clear();
                }
            }
            window.location.reload();
        }
    };
    //Save object into the localstorage 
    var btnsave = $(".k-grid-update");
    //var btnsave = document.getElementById('btnsave');
    btnsave.addEventListener('click', applogic.saveitem, false);
    //Clear all UI Elements 
    var btnclear = document.getElementById('btnclear');
    btnclear.addEventListener('click', applogic.clearuielements, false);
    //Clear LocalStorage 
    var btnclearstorage = document.getElementById('btnclearstorage');
    btnclearstorage.addEventListener('click', applogic.clearstorage, false);
    //On Load of window load data from local storage 
    window.onload = function () {
        applogic.loaddata();
    };
})();