var cCard = {
    props: ['competence'],
    template: `
    <div class="card" style="width: 12rem; cursor: pointer;" v-if="competence.Level > 0"> 
        <h5 class="card-title text-center text-uppercase">{{competence.Name}}</h5>
        <img class="card-img-top" :src="'assets/' + competence.Picture">
        <div class="card-body">
            <p class="card-text font-weight-bold"
                v-bind:class="{'competence-low': competence.Level < 2, 'competence-high': competence.Level > 3}">
                Poziom: {{competence.Level}}
            </p>
            <p class="card-text" v-if="competence.Description">Opis: {{competence.Description}}</p>
        </div>
    </div>
    `
};