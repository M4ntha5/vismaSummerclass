class App {

    constructor(selector){
        this.appElement = document.querySelector(selector);
        this.componentsByName = {};
    }

    addComponent(component) {
        this.componentsByName[component.name] = component;
      }
}