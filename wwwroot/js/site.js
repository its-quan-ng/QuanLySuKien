// EventHub - Site-wide JavaScript
// ===================================

// Wait for DOM to be ready
document.addEventListener('DOMContentLoaded', function() {
    initEventListingPage();
});

// ===================================
// EVENT LISTING PAGE FUNCTIONALITY
// ===================================

function initEventListingPage() {
    // Check if we're on the event listing page
    if (!document.querySelector('.event-cards-grid')) return;

    initFavoriteButtons();
    initSearchFunctionality();
    initFilterFunctionality();
    initSortFunctionality();
    initCardClickNavigation();
}

// Favorite Button Toggle
function initFavoriteButtons() {
    const favoriteBtns = document.querySelectorAll('.event-favorite-btn');

    favoriteBtns.forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();

            this.classList.toggle('active');
            const icon = this.querySelector('i');
            icon.classList.toggle('bi-star');
            icon.classList.toggle('bi-star-fill');

            // Optional: Save to localStorage or send to server
            const eventId = this.dataset.eventId;
            toggleFavoriteInStorage(eventId);
        });
    });

    // Load favorites from localStorage on page load
    loadFavoritesFromStorage();
}

function toggleFavoriteInStorage(eventId) {
    let favorites = JSON.parse(localStorage.getItem('eventFavorites') || '[]');
    const index = favorites.indexOf(eventId);

    if (index > -1) {
        favorites.splice(index, 1);
    } else {
        favorites.push(eventId);
    }

    localStorage.setItem('eventFavorites', JSON.stringify(favorites));
}

function loadFavoritesFromStorage() {
    const favorites = JSON.parse(localStorage.getItem('eventFavorites') || '[]');

    favorites.forEach(eventId => {
        const btn = document.querySelector(`.event-favorite-btn[data-event-id="${eventId}"]`);
        if (btn) {
            btn.classList.add('active');
            const icon = btn.querySelector('i');
            icon.classList.remove('bi-star');
            icon.classList.add('bi-star-fill');
        }
    });
}

// Search Functionality
function initSearchFunctionality() {
    const searchInput = document.getElementById('searchKeyword');
    const locationSelect = document.getElementById('searchLocation');

    if (!searchInput) return;

    // Debounce search input
    let searchTimeout;
    searchInput.addEventListener('input', function() {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            filterEvents();
        }, 300);
    });

    if (locationSelect) {
        locationSelect.addEventListener('change', filterEvents);
    }
}

// Filter Functionality
function initFilterFunctionality() {
    const filterCheckboxes = document.querySelectorAll('.filter-checkbox input[type="checkbox"]');

    filterCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', filterEvents);
    });
}

// Sort Functionality
function initSortFunctionality() {
    const sortSelect = document.getElementById('sortSelect');

    if (!sortSelect) return;

    sortSelect.addEventListener('change', function() {
        sortEvents(this.value);
    });
}

// Main Filter Function
function filterEvents() {
    const searchKeyword = document.getElementById('searchKeyword')?.value.toLowerCase() || '';
    const locationValue = document.getElementById('searchLocation')?.value.toLowerCase() || '';

    // Get selected filters
    const selectedPrices = getSelectedFilters('price');
    const selectedDates = getSelectedFilters('date');
    const selectedCategories = getSelectedFilters('category');
    const selectedFormats = getSelectedFilters('format');

    // Get all event cards
    const eventCards = document.querySelectorAll('.event-card');
    let visibleCount = 0;

    eventCards.forEach(card => {
        let shouldShow = true;

        // Search keyword filter
        if (searchKeyword) {
            const title = card.querySelector('.event-card-title a')?.textContent.toLowerCase() || '';
            const venue = card.querySelector('.event-card-venue span')?.textContent.toLowerCase() || '';

            if (!title.includes(searchKeyword) && !venue.includes(searchKeyword)) {
                shouldShow = false;
            }
        }

        // Location filter
        if (locationValue) {
            const venue = card.querySelector('.event-card-venue span')?.textContent.toLowerCase() || '';
            if (!venue.includes(locationValue)) {
                shouldShow = false;
            }
        }

        // Price filter
        if (selectedPrices.length > 0) {
            const priceText = card.querySelector('.event-card-price span')?.textContent || '';
            const isFree = priceText.includes('Miễn phí');
            const isPaid = !isFree && priceText !== 'Liên hệ';

            const showFree = selectedPrices.includes('free') && isFree;
            const showPaid = selectedPrices.includes('paid') && isPaid;

            if (!showFree && !showPaid) {
                shouldShow = false;
            }
        }

        // Category filter (if needed, match with data-loai attribute)
        if (selectedCategories.length > 0) {
            const category = card.dataset.loai?.toLowerCase() || '';
            // For now, just show all if any category is selected
            // You can implement more specific matching later
        }

        // Show/hide card
        if (shouldShow) {
            card.style.display = '';
            visibleCount++;
        } else {
            card.style.display = 'none';
        }
    });

    // Show "no results" message if needed
    showNoResultsMessage(visibleCount);
}

// Sort Events
function sortEvents(sortBy) {
    const grid = document.querySelector('.event-cards-grid');
    if (!grid) return;

    const cards = Array.from(grid.querySelectorAll('.event-card'));

    cards.sort((a, b) => {
        switch(sortBy) {
            case 'date':
                // Sort by date badge
                const dateA = a.querySelector('.date-day')?.textContent || '0';
                const dateB = b.querySelector('.date-day')?.textContent || '0';
                return parseInt(dateA) - parseInt(dateB);

            case 'price-low':
                // Sort by price ascending
                const priceA = extractPrice(a);
                const priceB = extractPrice(b);
                return priceA - priceB;

            case 'price-high':
                // Sort by price descending
                const priceADesc = extractPrice(a);
                const priceDescB = extractPrice(b);
                return priceDescB - priceADesc;

            default: // relevance
                return 0;
        }
    });

    // Re-append cards in sorted order
    cards.forEach(card => grid.appendChild(card));
}

// Helper Functions
function getSelectedFilters(filterName) {
    const checkboxes = document.querySelectorAll(`input[name="${filterName}"]:checked`);
    return Array.from(checkboxes).map(cb => cb.value);
}

function extractPrice(card) {
    const priceText = card.querySelector('.event-card-price span')?.textContent || '';

    if (priceText.includes('Miễn phí')) return 0;
    if (priceText.includes('Liên hệ')) return 999999999;

    // Extract number from "123,456 VNĐ"
    const match = priceText.match(/[\d,]+/);
    if (match) {
        return parseInt(match[0].replace(/,/g, ''));
    }

    return 0;
}

function showNoResultsMessage(visibleCount) {
    let noResultsMsg = document.querySelector('.no-results-message');

    if (visibleCount === 0) {
        if (!noResultsMsg) {
            const grid = document.querySelector('.event-cards-grid');
            noResultsMsg = document.createElement('div');
            noResultsMsg.className = 'no-results-message col-12';
            noResultsMsg.innerHTML = `
                <div class="alert alert-info text-center">
                    <i class="bi bi-info-circle me-2"></i>
                    Không tìm thấy sự kiện nào phù hợp. Vui lòng thử lại với bộ lọc khác!
                </div>
            `;
            grid.appendChild(noResultsMsg);
        }
        noResultsMsg.style.display = 'block';
    } else {
        if (noResultsMsg) {
            noResultsMsg.style.display = 'none';
        }
    }
}
