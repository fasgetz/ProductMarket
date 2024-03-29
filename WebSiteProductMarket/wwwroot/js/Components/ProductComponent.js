// Компонент продукта
Vue.component('product', {
    props: ['data'],
    template: `
                <div class="col-md-6 col-lg-4 col-xl-2 Product"">
                    <div class="Product__procent">
                        <span v-if="item.discountProduct[0] != null">{{item.discountProduct[0].procentDiscount}}%</span>
                    </div>
                    <div class="Product__Poster text-center">
                        <img v-bind:src="getImage(item.poster)" />
                    </div>
                    <div class="Product__title text-center">
                        <a :href="'/Product/About?id=' + item.id">{{item.name}}</a>
                    </div>
                    <div class="Product__count text-center p-3">
                        <span>{{item.description}}</span>
                    </div>
                    <div v-if="item.idSubCategoryNavigation != null" class="Product__category">
                        <a v-bind:href="'/category/search?category='+ item.idSubCategoryNavigation.id">
                            <span>{{item.idSubCategoryNavigation.name}}</span>
                        </a>
                    </div>
                    <div class="Product__price mt-3">
                        <div class="container-fluid">
                            <div v-if="item.discountProduct[0] == null" class="row">
                                <div class="col p-0 text-center"><b><span>{{priceCurrency}}</span></b> {{currency}}/копия</div>
                            </div>
                            <div v-else class="row">
                                <div class="col-7 p-0"><span>{{(priceCurrency - (priceCurrency / 100 * item.discountProduct[0].procentDiscount)).toFixed(2)}}</span> {{currency}}/ед.</div>
                                <div class="col p-0 oldPrice">{{priceCurrency}} <b>{{currency}}</b></div>
                            </div>
                        </div>
                    </div>
                    <div class="Product__economy">
                        <p v-if="item.discountProduct[0] != null"><span class="Product__economy__procent">-{{item.discountProduct[0].procentDiscount}}%</span> <span class="Product__economy__info">Экономия {{(item.price / 100 * item.discountProduct[0].procentDiscount).toFixed(2)}} <b>{{currency}}</b></span></p>
                    </div>
                    <div class="Product__basket">
                        <div class="container-fluid p-0">
                            <div class="mb-3">
                                <select v-model="currency" class="browser-default custom-select">
                                  <option value="EUR" selected>EUR</option>
                                  <option value="USD">USD</option>
                                  <option value="RUB">RUB</option>
                                  <option value="BGN">BGN</option>
                                </select>
                            </div>
                            <div v-if="addToBasket == false">
                                <div class="row">
                                    <div class="col p-0 basket__detail">
                                        <div class="container">
                                            <div class="row">
                                                <div class="col-4 p-0"><input class="btn btn-primary w-100" v-on:click="decrement" type="button" value="-"></div>
                                                <div class="col-4 p-0 text-center d-flex align-items-center justify-content-center"><span>{{count}}</span></div>
                                                <div class="col-4 p-0"><input class="btn btn-primary w-100" v-on:click="count++" type="button" value="+"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col p-0">
                                        <input class="btn btn-danger w-100" type="button" v-on:click="AddProduct"  value="В КОРЗИНУ">
                                    </div>
                                </div>
                                <div class="basket__totalSum">
                                    <p class="text-center">
                                        <span v-if="item.discountProduct[0] == null">{{count}}x{{priceCurrency}} = {{priceCurrency * count}} </span>
                                        <span v-else>{{count}}x{{priceCurrency - (priceCurrency / 100 * item.discountProduct[0].procentDiscount)}} = {{((priceCurrency - (priceCurrency / 100 * item.discountProduct[0].procentDiscount)) * count).toFixed(2)}} </span>
                                        <span><b>{{currency}}</b></span>
                                    </p>
                                </div>
                            </div>
                            <div class="Product__basket__buttons" v-else>
                                    <button v-on:click="GoPay" class="btn btn-success w-100">ОПЛАТИТЬ
                                        <span v-if="item.discountProduct[0] == null">{{count}}x{{priceCurrency}} = {{priceCurrency * count}} </span>
                                        <span v-else>{{count}}x{{priceCurrency - (priceCurrency / 100 * item.discountProduct[0].procentDiscount)}} = {{((priceCurrency - (priceCurrency / 100 * item.discountProduct[0].procentDiscount)) * count).toFixed(2)}} </span>
                                        <span><b>{{currency}}</b></span>
                                    </button>
                                    <button class="btn btn-danger w-100" v-on:click="cancel">ОТМЕНИТЬ</button>
                            </div>
                        </div>
                    </div>
                </div>
`,
    data: function () {
        return {
            count: 1,
            item: this.data,
            auth: false,
            addToBasket: false,
            currency: "EUR"
        }
    },
    computed: {
        priceCurrency: function () {



            if (this.currency == "EUR") {
                return this.item.price
            }
            else if (this.currency == "USD") {
                return this.item.price * 1.19
            }
            else if (this.currency == "RUB") {
                return this.item.price * 87
            }
            else if (this.currency == "BGN") {
                return this.item.price * 2
            }


            return 355;
        },
        totalSum: function () {

            var discount = this.item.discountProduct[0] // Скидка
            var totalPrice = 0

            if (discount != null) {
                totalPrice = ((this.item.price - (this.item.price / 100 * this.item.discountProduct[0].procentDiscount)) * this.count)
            }
            else {
                //alert('nodis')
                totalPrice = (this.item.price * this.count)
            }


            return totalPrice;
        }
    },
    mounted() {
        // Функция которая загрузит инфу, добавлен ли итем в корзину
        axios.all([
            this.HasItem()
        ])
            .then(axios.spread((first_response) => {
                if (first_response != null) {

                    this.addToBasket = true;
                    this.count = first_response.count;
                }

            }))


        //this.authorized = @User.Identity.IsAuthenticated.ToString();
    },
    methods: {
        GoPay: function () {
            location.href = "/Basket/Payment?currency=" + this.currency;
        },
        HasItem: function () {
            return axios.get(('/Basket/haveItem'), {
                params: {
                    idProduct: this.item.id
                }
            })
                .then(response => {
                    var exist = response.data;

                    return exist;
                });
        },
        removeProduct: function () {
            return axios.get(('/Basket/Remove'), {
                params: {
                    idProduct: this.item.id
                }
            })
                .then(function (response) {
                    return response.data;
                });
        },
        cancel: function () {
            axios.all([
                this.removeProduct()
            ])
                .then(axios.spread((first_response) => {
                    if (first_response == true)
                        this.addToBasket = false;
                    //this.addToBasket = first_response;
                }))
        },
        decrement: function () {
            if (this.count > 1) {
                this.count--;

            }

        },
        ExistProduct: function () {
            return axios.get((urlApi + 'api/product/ExistProduct'), {
                params: {
                    idProduct: this.item.id
                }
            })
                .then(function (response) {
                    //window.location.href = urlApp + "admin/"
                    var exist = response.data;

                    return exist;
                });
        },
        Add: function () {
            // Форма
            let formData = new FormData();
            formData.append('id', this.item.id);
            formData.append('count', this.count);
            //formData.append('Price', this.postBody.Price);
            //formData.append('Amount', this.postBody.Amount);
            //formData.append('idSubCategoryProduct', this.postBody.idSubCategoryProduct);


            axios.post(('/Basket/Add'),
                formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                }
            })
                .then(function (response) {
                    //window.location.href = urlApp + "admin/"
                    console.log(response);

                    //alert(response);
                });
        },
        // Добавление продукта в корзину
        AddProduct: function () {
            event.preventDefault();

            axios.all([
                this.ExistProduct()
            ])
                .then(axios.spread((first_response) => {
                    // Если продукт есть в базе данных, то добавить в корзину
                    if (first_response == true) {
                        this.Add();
                        this.addToBasket = true;
                    }

                }))
        },
        getImage: function (array) {
            return 'data:image/jpg;base64, ' + array
        }
    }
})
