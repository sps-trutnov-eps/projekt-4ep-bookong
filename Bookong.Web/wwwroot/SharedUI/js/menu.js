(function () {
    // Use event delegation so listeners survive Blazor DOM updates
    function openOverlay(overlay, toggle) {
        if (!overlay) return;
        overlay.classList.add('active');
        document.body.classList.add('menu-open');
        try { toggle && toggle.setAttribute('aria-expanded', 'true'); } catch (e) {}
    }

    function closeOverlay(overlay, toggle) {
        if (!overlay) return;
        overlay.classList.remove('active');
        document.body.classList.remove('menu-open');
        try { toggle && toggle.setAttribute('aria-expanded', 'false'); } catch (e) {}
    }

    // Toggle when clicking the hamburger (delegated)
    document.addEventListener('click', function (e) {
        var toggle = e.target.closest && e.target.closest('#menuToggle');
        if (toggle) {
            e.preventDefault();
            var overlay = document.getElementById('overlayMenu');
            if (!overlay) return;
            if (overlay.classList.contains('active')) closeOverlay(overlay, toggle);
            else openOverlay(overlay, toggle);
            return;
        }

        // Close button inside overlay
        var closeBtn = e.target.closest && e.target.closest('#closeMenu');
        if (closeBtn) {
            var overlay = document.getElementById('overlayMenu');
            closeOverlay(overlay, document.getElementById('menuToggle'));
            return;
        }

        // Click on overlay background closes it
        var overlayEl = e.target.closest && e.target.closest('#overlayMenu');
        if (overlayEl && e.target === overlayEl) {
            closeOverlay(overlayEl, document.getElementById('menuToggle'));
            return;
        }

        // Click on any overlay link closes it
        var navLink = e.target.closest && e.target.closest('.overlay-nav-link');
        if (navLink) {
            closeOverlay(document.getElementById('overlayMenu'), document.getElementById('menuToggle'));
            return;
        }
    }, true);

    // Escape key closes overlay
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            var overlay = document.getElementById('overlayMenu');
            if (overlay && overlay.classList.contains('active')) {
                closeOverlay(overlay, document.getElementById('menuToggle'));
            }
        }
    });
})();
