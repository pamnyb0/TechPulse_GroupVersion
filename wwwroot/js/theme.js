// theme.js

// läser temat från localstorage
let currentTheme = localStorage.getItem('theme') || 'light';
document.documentElement.setAttribute('data-theme', currentTheme);

function updateTheme() {
    document.body.classList.toggle('dark-mode', currentTheme === 'dark');
    document.body.classList.toggle('light-mode', currentTheme === 'light');

    document.querySelectorAll('#moon, #sun').forEach(themeIcon => {
        themeIcon.src = currentTheme === 'light' ? '/icons/sun.svg' : '/icons/svg.svg';
        themeIcon.alt = currentTheme === 'light' ? 'Ljust läge ikon' : 'Mörkt läge ikon';

    });
}
// När sidan laddas, tillämpa det sparade temat
document.addEventListener('DOMContentLoaded', updateTheme);

// Växla tema när användaren klickar på temaknappen
const themeToggle = document.getElementById('theme-toggle-btn');
if (themeToggle) {
    themeToggle.addEventListener('click', () => {
        currentTheme = currentTheme === 'light' ? 'dark' : 'light';
        localStorage.setItem('theme', currentTheme); // Spara temat i localStorage
        updateTheme(); // Uppdatera temat och ikonen
    });
}
