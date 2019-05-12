var menuBar = {
  template: `
  <div>
  <nav class="navbar navbar-light bg-light">
  <form class="form-inline">
    <button class="btn btn-outline-success" type="button" v-on:click="testsList" :disabled="!isLogged">Lista Testów</button>
    <button class="btn btn-outline-success" type="button" v-on:click="newTest" :disabled="!isLogged">Nowy Test</button>
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
      window.location.replace("testList.html");
    },
    logOut: function(){
      sessionStorage.removeItem("tokenKey");
      window.location.replace("index.html");
    },
    newTest: function(){
        var self = this;
        $.ajax({
          type: "PUT",
          url: backend + "/api/create/",
          headers: {
              'Authorization': `Bearer ${sessionStorage.getItem("tokenKey")}`,
              'Access-Control-Allow-Origin': '*'
          },
          timeout: 5000,

          data: JSON.stringify({ "Name": "", "Desc": "" }),
          contentType: 'application/json; charset=utf-8',
          dataType: 'json',
        }).done(function (id) {
          window.location.replace("editTest.html?id="+id);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
        })
    }
  },
};