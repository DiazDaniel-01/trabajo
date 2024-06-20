document.addEventListener('DOMContentLoaded', () => {
    const cartItems = document.getElementById('cart-items');
    const cartTotal = document.getElementById('cart-total');
    let cart = [];

    function bindAddToCartButtons() {
        const addToCartButtons = document.querySelectorAll('.add-to-cart');
        addToCartButtons.forEach(button => {
            button.addEventListener('click', () => {
                const name = button.getAttribute('data-name');
                const price = parseFloat(button.getAttribute('data-price'));
                addItemToCart(name, price);
                displayCart();
            });
        });
    }

    function addItemToCart(name, price) {
        for (let item of cart) {
            if (item.name === name) {
                item.quantity++;
                return;
            }
        }
        cart.push({ name, price, quantity: 1 });
    }

    function displayCart() {
        cartItems.innerHTML = '';
        let total = 0;
        for (let item of cart) {
            const listItem = document.createElement('li');
            listItem.className = 'list-group-item d-flex justify-content-between align-items-center cart-item';
            listItem.innerHTML = `
                ${item.name} - $${item.price.toFixed(2)}
                <div>
                    <span class="badge badge-primary badge-pill">${item.quantity}</span>
                    <span class="badge badge-danger badge-pill ml-2 remove-item" data-name="${item.name}">&times;</span>
                </div>
            `;
            cartItems.appendChild(listItem);
            total += item.price * item.quantity;
        }
        cartTotal.textContent = total.toFixed(2);

        // Add event listeners to remove buttons
        const removeButtons = document.querySelectorAll('.remove-item');
        removeButtons.forEach(button => {
            button.addEventListener('click', () => {
                const name = button.getAttribute('data-name');
                removeItemFromCart(name);
                displayCart();
            });
        });
    }

    function removeItemFromCart(name) {
        for (let i = 0; i < cart.length; i++) {
            if (cart[i].name === name) {
                cart[i].quantity--;
                if (cart[i].quantity === 0) {
                    cart.splice(i, 1);
                }
                return;
            }
        }
    }

    // Bind the add to cart buttons initially
    bindAddToCartButtons();
});
