// IIFE - Immediately Invoked Function Expression
// * Encapsulates variables, preventing conflicts with global variables.
// * One-time initialization, ideal for one-time tasks like this.
(() => {
    // Maps of translations
    const loadingTranslations = {
        en: 'Loading',
        cs: 'Načítání'
    };

    const reloadTranslations = {
        en: 'Reload',
        cs: 'Obnovit'
    }

    const unhandledErrorTranslations = {
        en: 'An unhandled error has occurred',
        cs: 'Vyskytla se neočekávaná chyba'
    }

    // Pick the best language.
    // Yes, it's based on one specific translations map. Put up with it!
    function pickLang() {
        // const langs = navigator.languages || [navigator.language || 'en'];
        const langs = ['cs']; // For now, just run in Czech.

        for (const l of langs) {
            const lc = l.toLowerCase();

            if (loadingTranslations[lc]) {
                return lc;
            }

            const base = lc.split('-')[0];

            if (loadingTranslations[base]) {
                return base;
            }
        }

        return 'en';
    }

    function setTextContent(lang, id, translations) {
        const element = document.getElementById(id);

        if (element) {
            element.textContent = translations[lang];
        }
    }

    const lang = pickLang();
    setTextContent(lang, 'loading', loadingTranslations);
    setTextContent(lang, 'reload', reloadTranslations);
    setTextContent(lang, 'unhandled-error', unhandledErrorTranslations);
})();
