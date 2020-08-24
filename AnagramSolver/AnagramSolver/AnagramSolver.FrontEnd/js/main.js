const app = new App('#app');

const router = new Router(app);
router.addRoute('about', '#/about');
router.addRoute('search', '#/search');
router.addRoute('words', '#/words');