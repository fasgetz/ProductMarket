Vue.component('edit-category', {
    props: ['id', 'name'],
    template: `    
<form>
    <div>
        <h1 class="text-center">Редактирование категории продуктов</h1>
        <div class="form-group">
            <label class="control-label">Название</label>
            <input id="nameCategory" :value="postBody.Name" name="Name" v-model="postBody.Name" class="form-control" />
        </div>
        <div class="form-group">
            <label for="control-label">Выберите постер категории</label>
            <div class="custom-file">
                <input accept="image/x-png,image/gif,image/jpeg" v-on:change="ImageLoad" type="file" class="custom-file-input" id="myInput" aria-describedby="myInput">
                <label class="custom-file-label" for="myInput">{{ FileName }}</label>
            </div>
        </div>
        <div class="form-group text-center">
            <input v-on:click="editCategory" class="btn btn-primary" value="Сохранить" type="submit" />
        </div>
        <div class="form-group text-center mt-1">
            <input class="btn btn-primary" value="Удалить (неактивен)" type="submit" />
        </div>
    </div>
</form>
`,
    data: () => ({
        postBody: {
            Name: null
        },
        FileName: 'Выберите файл',
        file: null
    }),
    mounted() {        
        this.postBody.Name = this.name;
    },
    methods: {
        editCategory: function (event) {
            event.preventDefault();
            let formData = new FormData();
            formData.append('file', this.file);
            formData.append('Name', this.postBody.Name);
            formData.append('IdCategory', this.id);

            console.log('>> formData >> ', formData);



            axios.post((urlApi + 'api/category/EditCategory'),
                formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                }
            })
                .then(function (response) {
                    window.location.href = urlApp + "AdminCategories"
                    console.log(response);
                })
                .catch(function (response) {

                });
        },
        ImageLoad: function (event) {

            var file = document.getElementById("myInput").files[0];
            this.FileName = file.name;
            this.file = file;
        }

    }
})