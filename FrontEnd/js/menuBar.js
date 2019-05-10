var menuBar = {
  template: `
  <div>
  <nav class="navbar navbar-light bg-light">
  <form class="form-inline">
    <button class="btn btn-outline-success" type="button" v-on:click="testsList" :disabled="!isLogged">Lista Testów</button>
    <button class="btn btn-outline-success" type="button" v-on:click="logOut" :disabled="!isLogged">Wyloguj</button>
  </form>
</nav>
  </div>
  `,
  computed: {
    isLogged: function(){
      console.log(sessionStorage.getItem("tokenKey"));
      if (sessionStorage.getItem("tokenKey"))
        return true;
      return false;
    }
  },
  methods: {
    testsList: function(){
      //window.location.replace("testList.html");
      window.location.replace("");
    },
    logOut: function(){
      sessionStorage.removeItem("tokenKey");
      window.location.replace("index.html");
    }
  },
};