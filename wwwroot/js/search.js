document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("search");

    if (searchInput) {

        searchInput.addEventListener("keydown", debounce(function (e) {
            if (e.key === "Enter") {
                e.preventDefault();

                const query = this.value.trim().toUpperCase();

                const products = [
                    { name: "iPhone 12", category: "iPhone", price: `7999kr` },
                    { name: "iPhone 12 Pro", category: "iPhone", price: `9999kr` },
                    { name: "iPhone 12 Pro Max", category: "iPhone", price: `10999kr` },
                    { name: "iPhone 11", category: "iPhone", price: `6999kr` }, 
                    { name: "Samsung Galaxy S21", category: "Samsung", price: `8999kr` },
                    { name: "Samsung Galaxy S21 Ultra", category: "Samsung", price: `10999kr` },
                    { name: "Samsung Galaxy S21 Plus", category: "Samsung", price: `9999kr` },
                    { name: "Samsung Galaxy S20", category: "Samsung", price: `7999kr` },
                    { name: "Samsung Galaxy S20 Ultra", category: "Samsung", price: `9999kr` },
                    { name: "Google Pixel 5", category: "Google", price: `7999kr` },
                    { name: "Google Pixel 4a", category: "Google", price: `4999kr` },

               ];

                const filteredProducts = products.filter(product =>
                    product.name.toUpperCase().includes(query)
                );

                console.log(filteredProducts);
            }
        }, 300));
    }
});

function debounce(func, wait) {
    let timeout;
    return function () {
        const context = this;
        const args = arguments;
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            func.apply(context, args);
        }, wait);
    };
}