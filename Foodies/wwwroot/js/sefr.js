
<script>
    document.querySelectorAll('.category-item button').forEach(item => {
        item.addEventListener('click', function(event) {
            event.preventDefault();
            var selectedCategory = this.closest('.category-item').getAttribute('data-category');
            var restaurantCards = document.querySelectorAll('.restaurant-card');

            restaurantCards.forEach(card => {
                var cuisineType = card.getAttribute('data-cuisine-type');
                if (cuisineType === selectedCategory || selectedCategory === 'All') {
                    card.style.display = 'block';
                } else {
                    card.style.display = 'none';
                }
            });
        });
    });

    document.getElementById('restaurantSearch').addEventListener('input', function() {
        var searchQuery = this.value.toLowerCase();
        var restaurantCards = document.querySelectorAll('.restaurant-card');

        restaurantCards.forEach(card => {
            var restaurantName = card.querySelector('.restaurant-name').textContent.toLowerCase();
            if (restaurantName.includes(searchQuery)) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    });

    document.addEventListener('DOMContentLoaded', function () {
        const rateButtons = document.querySelectorAll('.rate-restaurant');

        rateButtons.forEach(button => {
            button.addEventListener('click', function () {
                const ratingInput = this.nextElementSibling;
                ratingInput.style.display = ratingInput.style.display === 'none' ? 'flex' : 'none';
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
</script>