var qCard = {
    props: ['question'],
    template: `
    <div>
        <p>Pytanie {{question.number+1}}: {{question.question}}</p>
        <form>
            <ul class="list-group">
                <li class="list-group-item" v-for="answer in question.answers">
                    <div class="radio" v-if="question.answersCount==1">
                        <div><input type="radio" name="optradio" v-model="question.selectedAnswer" :value=answer.number>{{answer.number}}{{answer.answer}}</div>
                    </div>
                    <div class="checkbox" v-else>
                        <div><input type="checkbox" v-model="answer.correct">{{answer.answer}}</div>
                    </div>
                </li>
            </ul>
        </form>
    </div>
    `
};