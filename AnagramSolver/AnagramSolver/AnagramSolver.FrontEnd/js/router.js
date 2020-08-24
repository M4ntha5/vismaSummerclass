class Router {
    constructor(app) {
      this.app = app;
      this.routes = [];
      this.hashChange = this.hashChange.bind(this);
  
      window.addEventListener('hashchange', this.hashChange);
    }
    addRoute(name, url) {
      this.routes.push({
        name,
        url
      });
    }
    hashChange() {
      const hash = window.location.hash;
      const route = this.routes.filter(route => hash.match(new RegExp(route.url)))[0];

      console.log(route);
  
    }
  }