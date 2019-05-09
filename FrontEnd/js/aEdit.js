var aEdit = {
    props: ['answer'],
    template: `
    <div>
        <td><input type="checkbox" aria-label="Checkbox for following text input" class="align-middle" v-model="answer.correct"></td>
        <td>
                    <textarea class="form-control rounded-0" rows="2" maxlength="1000" resize="false" style="resize:none"
        v-model="answer.answer"></textarea></td>
    </div>
    `,
}