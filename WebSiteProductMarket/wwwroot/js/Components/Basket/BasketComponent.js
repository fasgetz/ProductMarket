
var basket = Vue.component('basket', {
    template: `
            <div v-if="productBasket.products.length != 0" class="basket-block container-fluid">
                <div class="basket-block__header text-center">
                    <h1 class="basket-block__header-h1 mb-3">Корзина товаров</h1>
                </div>
                <div class="basket-block__table text-center">
                    <div class="basket-block__table-header row m-0">
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                           
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Название
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Количество
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Наличие
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            Стоимость
                        </div>
                        <div class="col-xl d-none d-xl-block basket-block__table-header__column">
                            
                        </div>
                    </div>
                    <div v-if="loadData == true">
                        <product-basket v-for="(item, index) in productBasket.products" :key="item.id" v-on:custom="removeHandler" :index="index" :data="item"></product-basket>
                    </div>
                    <div class="basket-block__table-result m-0 row">
                        <div class="order-md-1 order-2 col-12 col-md-6 align-self-end">
                            <div class="text-left justify-content-center justify-content-md-start d-flex">
                                <button v-on:click="GoIndex" class="btn btn-primary">Вернуться к покупкам</button>
                            </div>
                        </div>
                        <div class="order-md-2 order-1 col-12 col-md-6 mt-md-0 mt-3 m-0 basket-block__table-result__statistic text-right">
                            <div>
                                Всего товаров: {{watchCount}}
                            </div>
                            <div class="basket-block__table-result__statistic__discount">
                                Скидка: {{watchDiscount.toFixed(2)}} руб.
                            </div>
                            <div>
                                Итого: {{watchTotalPrice.toFixed(2)}} руб.
                            </div>
                        </div>

                    </div>
                    <div>
                            <h3 class="mt-3">Оформление заказа</h3>
                            <div>
                                <ul>
                                  <li>Условия предоставления услуг <input type="checkbox" v-model="cbUsl"></li>
                                  <li>Условия возврата денежных средств <input type="checkbox" v-model="cbVozr"></li>
                                  <li></li>
                                </ul>
                            </div>
                            <p>{{cbUsl}} && {{cbVozr}}</p>
                            <div class="col text-center payment-buttons-block" v-bind:class="[cbUsl == true && cbVozr == true ? '' : 'disabled-block']">
                                <button v-on:click="GoPay" class="btn btn-warning">Оформить заказ</button>                                
                                <button v-on:click="GoPay" class="btn btn-warning">Оформить заказ</button>
                                <a href="https://yandex.ru"><img class="img-payment" src="/Images/PaypalButton.png" alt="paypal"/></a>
                                <button v-on:click="PaymentPayPal" class="btn btn-primary">Оплатить</button>
                            </div>
                    </div>
                </div>
            </div>
            <div v-else class="basket-block container-fluid">
                <div class="basket-block__header text-center">
                    <h1 class="basket-block__header-h1">Корзина товаров пуста</h1>
                    <div class="basket-block__go-back">
                        <button v-on:click="GoIndex" class="btn btn-primary">Вернуться к покупкам</button>
                    </div>
                </div>
            </div>
            `,
    data: () => ({
        productBasket: null,
        loadData: false,
        totalCount: 0,
        totalPrice: 0,
        totalDiscount: 0,
        cbUsl: null,
        cbVozr: null
    }),
    methods: {

        clearBasket: function () {
            // Очищаем кеш добавленных продуктов
            axios.post(('ClearBasket'))
                .then(
                    response => {

                    });
        },
        addOrderPost: function () {
            var data = {
                basket: this.productBasket,
                //address: this.address,
                //commentary: this.commentary
            }

            return axios.post((urlApi + 'api/basket/PayPalPayment'), data)
                .then(
                    response => {

                        return response.data;

                    }).catch(error => {

                        //// Если ошибка 401, то пользователь не авторизован
                        if (error.response.status == 401) {
                            $('#loginButton').click();
                        }
                        //else {
                        //    $("#form-pay").validate({
                        //        rules: {
                        //            address: {
                        //                required: true,
                        //                minlength: 10
                        //            }
                        //        },
                        //        messages: {
                        //            address: {
                        //                required: "Поле обязательно для заполнения",
                        //                minlength: jQuery.validator.format("Длина адреса доставки должна быть больше 10-ти символов")
                        //            }
                        //        }
                        //    });
                        //}
                    });
        },
        // Оплата пайпел
        PaymentPayPal: function () {
            event.preventDefault();

            axios.all([
                this.GetCart()
            ])
                // Если успешно из кэша получили корзину, то необхоимо оплатить пайпел
                .then(axios.spread((first_response) => {
                    if (first_response == true) {

                        axios.all([
                            this.addOrderPost()
                        ])
                            // Если успешно сгенерировали ссылку на пайпел, то необходимо перейти по адресу
                            .then(axios.spread((two_response) => {
                                if (two_response != null) {
                                    // Очищаем кеш
                                    //this.clearBasket();

                                    // Присваиваем свойства
                                    //this.productBasket = two_response;
                                    //this.added = true;

                                    window.location.href = two_response
                                    //alert('переходим по адресу оплаты заказа')
                                }
                            }))

                    }
                }))



        },

/*        AuthUser: function () {
            $('#loginButton').click();
        },*/
        GoPay: function () {
            window.location.href = "/basket/pay"
        },
        GoIndex: function () {
            window.location.href = "/Home/Index"
        },
        GetCart: function () {
            // Загружаем из кеша, что добавлено в корзину
            return axios.get(('GetCartJson'))
                .then(
                    response => {
                        this.productBasket = response.data;
                        return true;
                    });
        },
        loadItems: function () {
            axios.post((urlApi + 'api/basket/getBasketProducts'), this.productBasket)
                .then(
                    response => {
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        this.productBasket = response.data;
                        // force update to make sure update hook fires
                        this.loadData = true;
                        /*this.authUser = true;*/
                    }).catch(error => {

                        // Если ошибка 401, то пользователь не авторизован
                        if (error.response.status == 401) {
                            /*this.authUser = false*/
                        }
                    });
        },
        removeHandler: function (id) {

            var pos = this.productBasket.products.map(function (e) { return e.id; }).indexOf(id);
            this.productBasket.products.splice(pos, 1);

        }
    },
    computed: {
        watchDiscount: function () {
            this.totalDiscount = 0;
            for (i = 0; i < this.productBasket.products.length; i++) {
                if (this.productBasket.products[i].procentDiscount != null) {
                    this.totalDiscount += ((this.productBasket.products[i].price / 100 * this.productBasket.products[i].procentDiscount)) * this.productBasket.products[i].count
                }
            }

            return this.totalDiscount;
        },
        watchCount: function () {
            this.totalCount = 0;
            for (i = 0; i < this.productBasket.products.length; i++) {
                this.totalCount += this.productBasket.products[i].count;
            }

            return this.totalCount;
        },
        watchTotalPrice: function () {
            this.totalPrice = 0;
            for (i = 0; i < this.productBasket.products.length; i++) {
                // Если у итема есть скидка, то посчитать с ней
                if (this.productBasket.products[i].DiscountProduct != null)
                    this.totalPrice += (this.productBasket.products[i].price - (this.productBasket.products[i].price / 100 * this.productBasket.products[i].DiscountProduct)) * this.productBasket.products[i].count
                else
                    this.totalPrice += this.productBasket.products[i].price * this.productBasket.products[i].count;
            }

            return this.totalPrice - this.totalDiscount;
        }
    },
    mounted() {

        axios.all([
            this.GetCart()
        ])
            .then(axios.spread((first_response) => {
                if (first_response == true) {
                    setTimeout(() => {
                        this.loadItems();
                    });
                }
            }))
    }
})
