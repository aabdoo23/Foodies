document.addEventListener('DOMContentLoaded', function () {
    const rateButtons = document.querySelectorAll('.rate-restaurant');

    rateButtons.forEach(button => {
        button.addEventListener('click', function () {
            const ratingInput = this.nextElementSibling;
            ratingInput.style.display = ratingInput.style.display === 'none' ? 'block' : 'none';
        });
    });

    const submitButtons = document.querySelectorAll('.submit-rating');

    submitButtons.forEach(button => {
        button.addEventListener('click', async function () {
            const restaurantId = this.getAttribute('data-restaurant-id');
            const ratingValue = this.previousElementSibling.value;

            if (ratingValue) {
                const response = await fetch(`/Menu/AddRating`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ RestaurantId: restaurantId, Rate: parseFloat(ratingValue) })
                });

                if (response.ok) {
                    alert('Thank you for your rating!');
                    location.reload();
                } else {
                    alert('An error occurred while submitting your rating.');
                }
            } else {
                alert('Please enter a rating.');
            }
        });
    });
});