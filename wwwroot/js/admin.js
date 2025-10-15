
document.addEventListener('DOMContentLoaded', function() {

    const counterElements = document.querySelectorAll('.stat-number, .dashboard-number');
    counterElements.forEach(element => {
        element.style.transition = 'none';
        element.style.animation = 'none';
    });

    const currentPath = window.location.pathname;
    const sidebarLinks = document.querySelectorAll('.sidebar .nav-link');
    
    sidebarLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });
});

document.querySelectorAll('a[href*="Delete"]').forEach(link => {
    link.addEventListener('click', function(e) {
        if (!confirm('Bạn có chắc chắn muốn xóa?')) {
            e.preventDefault();
        }
    });
});

if (window.location.pathname.includes('/Admin')) {
    setInterval(() => {
    }, 30000);
}

