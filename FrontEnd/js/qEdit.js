var qEdit = {
    props: ['question'],
    template: `
    <div>
        <label>Pytanie Nr: {{question.number + 1}}</label>
        
        <div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text" id="inputGroup-sizing-default">Pytanie</span>
            </div>
            <textarea class="form-control rounded-0" rows="3" maxlength="1000" resize="false" style="resize:none"
            placeholder="treść pytania" v-model="question.question"></textarea>
        </div>
    </div>
    `
};