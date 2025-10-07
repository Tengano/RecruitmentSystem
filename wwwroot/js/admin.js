// Admin panel JavaScript

// Disable counter animations for admin dashboard
document.addEventListener('DOMContentLoaded', function() {
    // Disable counter animations
    const counterElements = document.querySelectorAll('.stat-number, .dashboard-number');
    counterElements.forEach(element => {
        element.style.transition = 'none';
        element.style.animation = 'none';
    });
    
    // Highlight current sidebar item
    const currentPath = window.location.pathname;
    const sidebarLinks = document.querySelectorAll('.sidebar .nav-link');
    
    sidebarLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });
});

// Confirm delete actions
document.querySelectorAll('a[href*="Delete"]').forEach(link => {
    link.addEventListener('click', function(e) {
        if (!confirm('Bạn có chắc chắn muốn xóa?')) {
            e.preventDefault();
        }
    });
});

// Auto-refresh dashboard stats every 30 seconds
if (window.location.pathname.includes('/Admin')) {
    setInterval(() => {
        // Optionally refresh stats via AJAX
    }, 30000);
}

