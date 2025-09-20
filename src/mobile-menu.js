document.addEventListener('DOMContentLoaded', function() {
  const mobileMenu = document.getElementById('navbarNav');
  const closeBtn = document.getElementById('closeMenu');

  // Close menu on close button click
  if (closeBtn) {
    closeBtn.addEventListener('click', function() {
      mobileMenu.classList.remove('show');
    });
  }

  // Close menu when clicking on a link
  if (mobileMenu) {
    mobileMenu.addEventListener('click', function(event) {
      if (event.target.classList.contains('nav-link')) {
        mobileMenu.classList.remove('show');
      }
    });
  }
});
