
var basket = Vue.component('basket', {
    props: ['currency'],
    template: `
            <div v-bind:class="[loading == true ? 'disabled-block' : '']" v-if="productBasket.products.length != 0" class="basket-block container-fluid">
<div v-if="loading == true" class="myspinner justify-content-center align-items-center d-flex" style="position: fixed;
  width: 80%;
height: 80%;
    z-index: 1">
  <div class="spinner-border text-primary" style="width: 4rem; height: 4rem;" role="status">
    <span class="sr-only">Loading...</span>
  </div>
</div>
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
                            Описание
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
                            <div class="text-left mt-3 mt-md-0 justify-content-center justify-content-md-start d-flex">
                                <button v-on:click="GoIndex" class="btn btn-primary">Вернуться к покупкам</button>
                            </div>
                        </div>
                        <div class="order-md-2 order-1 col-12 col-md-6 mt-md-0 mt-3 m-0 basket-block__table-result__statistic text-right">
                            <div>
                                Всего товаров: {{watchCount}}
                            </div>
                            <div class="basket-block__table-result__statistic__discount">
                                Скидка: {{watchDiscount.toFixed(2)}} {{currencyVal}}
                            </div>
                            <div>
                                Итого: {{watchTotalPrice.toFixed(2)}} {{currencyVal}}
                            </div>
                        </div>

                    </div>
                    <div>

                            <h3 class="mt-3">Оформление заказа</h3>
                            <div class="form-group">
                                <textarea maxlength="150" placeholder="Введите комментарий к оплате" v-model="commentary" class="form-control" rows="7"></textarea>
                            </div>
                            <div class="mb-3">
                                <select v-model="currencyVal" class="browser-default custom-select">
                                  <option value="EUR">EUR</option>
                                  <option value="USD">USD</option>
                                  <option value="RUB">RUB</option>
                                  <option value="BGN">BGN</option>
                                </select>
                            </div>
                            <div>
                                <ul style="font-size: 16px">
                                  <li class="row">
                                    <div class="col d-flex justify-content-end"><a style="color: blue" href="/Oferta/Oferta">Условия предоставления услуг</a></div>
                                    <div class="col-2 col-md d-flex justify-content-start"><input  style="width: 20px; height: 20px;" type="checkbox" v-model="cbUsl"></div>
                                  </li>
                                  <li class="row mt-1">
                                    <div class="col d-flex justify-content-end"><a style="color: blue" href="/Oferta/Refund">Условия возврата денежных средств</a></div>
                                    <div class="col-2 col-md d-flex justify-content-start"><input  style="width: 20px; height: 20px;" type="checkbox" v-model="cbVozr"></div>
                                  </li>                                  
                                </ul>
                            </div>
                            <div class="col text-center payment-buttons-block" v-bind:class="[cbUsl == true && cbVozr == true ? '' : 'disabled-block']">

                                <button v-on:click="PaymentStripe" class="btn btn-success" style="width: 172.55px; height: 60px; font-size: 20px">Оплатить</button>
                                <button v-on:click="PaymentPayPal" class="btn"><img class="img-payment" src="/Images/PaypalButton.png" alt="paypal"/></button>
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
        cbVozr: null,
        commentary: null,
        loading: false,
        currencyVal: null
    }),
    methods: {

        clearBasket: function () {
            // Очищаем кеш добавленных продуктов
            axios.post(('ClearBasket'))
                .then(
                    response => {

                    });
        },
        addOrderStripe: function () {
            this.loading = true

            var data = {
                basket: this.productBasket,
                //address: this.address,
                commentary: this.commentary
            }

            return axios.post((urlApi + 'api/basket/PaymentStripe'), data)
                .then(
                    response => {

                        return response.data;

                    }).catch(error => {

                        //// Если ошибка 401, то пользователь не авторизован
                        if (error.response.status == 401) {
                            $('#loginButton').click();
                        }
                    });
        },
        addOrderPost: function () {
            this.loading = true

            var data = {
                basket: this.productBasket,
                //address: this.address,
                commentary: this.commentary
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
                    });
        },
        // Оплата Stripe
        PaymentStripe: function () {
            event.preventDefault();

            axios.all([
                this.GetCart()
            ])
                // Если успешно из кэша получили корзину, то необхоимо оплатить пайпел
                .then(axios.spread((first_response) => {
                    if (first_response == true) {

                        axios.all([
                            this.addOrderStripe()
                        ])
                            // Если успешно сгенерировали ссылку на пайпел, то необходимо перейти по адресу
                            .then(axios.spread((two_response) => {
                                if (two_response != null) {
                                    // Очищаем кеш
                                    //this.clearBasket();

                                    // Присваиваем свойства
                                    //this.productBasket = two_response;
                                    //this.added = true;

                                    //alert('переходим по адресу оплаты заказа')
                                    window.location.href = two_response
                                }
                            }))

                    }
                }))



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


            // Теперь в зависимости от валюты необходимо посчитать соотношение
            if (this.currencyVal == "EUR") {
                return this.totalPrice - this.totalDiscount
            }
            else if (this.currencyVal == "USD") {
                return (this.totalPrice - this.totalDiscount) * 1.19
            }
            else if (this.currencyVal == "BGN") {
                return (this.totalPrice - this.totalDiscount) * 2
            }
            else if (this.currencyVal == "RUB") {
                return (this.totalPrice - this.totalDiscount) * 87
            }

            return this.totalPrice - this.totalDiscount;
        }
    },
    mounted() {

        this.currencyVal = this.currency

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
