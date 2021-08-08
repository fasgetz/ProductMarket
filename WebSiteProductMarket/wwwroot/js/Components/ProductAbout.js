// Компонент продукта
Vue.component('product', {
    props: ['data'],
    template: `


<div class="w-100 row Product">
                <div class="col-12 col-xl-4 Product" style="margin-top: 0px !important;">
                    <div class="Product__title text-center" style="font-size: 22px !important">
                        {{item.name}}
                    </div>
                    <div class="Product__Poster text-center">
                        <img v-bind:src="getImage(item.poster)" />
                    </div>
                    <div class="Product__count text-center p-3">
                        <span>{{item.description}}</span>
                    </div>
                    <div v-if="item.idSubCategoryNavigation != null" class="Product__category">
                        <a v-bind:href="'/category/search?category='+ item.idSubCategoryNavigation.id">
                            Категория: <span>{{item.idSubCategoryNavigation.name}}</span>
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
                <div class="col-12 col-xl-8">
                    <div class="container">
                        <h3>Варианты покупки</h3>
                        <p>Внимание! Вы можете активировать приобретенное ПО по серийному ключу, который предоставляется после покупки.
                            Дополнительные языковые пакеты можно скачать опционально после установки программы на ПК.</p>
                        <p>
                            Языки интерфейса: Русский, Английский
                        </p>
                        <p>
                            Операционные системы: Windows 10, Windows 8.1, macOS, Android, LinuxOS
                        </p>
                        <p>
                            Способ доставки: электронная доставка
                        </p>
                    </div>
                    <div class="container mt-3">
                        <h3>Системные требования</h3>
                        <p>Процессор: 32- или 64-разрядный процессор с тактовой частотой 1 ГГц или выше с набором инструкций SSE2.</p>
                        <p>
                            Операционная система: Windows 10, Windows 8.1 или 8, последние две версии macOS, Android Nougat 7.0﻿﻿
                        </p>
                        <p>
                            Оперативная память: 1 ГБ (для 32-разрядных систем); 2 ГБ (для 64-разрядных систем)
                        </p>
                        <p>
                            Свободное место на жестком диске: 3 ГБ
Монитор: Для использования аппаратного ускорения требуется видеоадаптер, поддерживающий DirectX 10 и разрешение 1024 x 576
                        </p>
                        <p>
Версия .NET: 3.5, 4.0 или 4.5
                        </p>
                        <p>
Мультисенсорный ввод: Устройство с сенсорным экраном должно поддерживать все функции мультисенсорного ввода.
                        </p>
                        <p>
Все функции и возможности также доступны при использовании клавиатуры, мыши или другого стандартного либо доступного устройства ввода. Обратите внимание, что новые функции сенсорного ввода оптимизированы для использования в ОС Windows 8.
                        </p>
                    </div>
                </div>
                <div class="col-12 mt-5">
                    <div class="container mt-3">
                        <h3 class="text-center">Отзывы продукции BusManSoft</h3>
                        <div class="col-12 mt-3">
                            <div class="container p-3" style="border: 1px solid rgb(208, 210, 212); border-radius: 25px;">
                                <div class="row">
                                    <div class="col-4 text-left">
                                        <h5>Евгений Сидорчук</h5>
                                    </div>
                                    <div class="col-8 text-muted text-right">
                                        24.05.2021
                                    </div>
                                </div>
                                <div class="row mt-1" style="padding-left: 15px; padding-right: 15px">
                                    <p> Начал пользоваться данным программным обеспечением на этапе демо версии. Всем доволен. Новые версии всегда имеют преимущества перед старыми, регулярные обновления продуктов добавляют удобство в их пользовании. Очень ускоряет работу возможность совместного редактирования документов, что в несколько раз увеличивает производительность.</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 mt-3">
                            <div class="container p-3" style="border: 1px solid rgb(208, 210, 212); border-radius: 25px;">
                                <div class="row">
                                    <div class="col-4 text-left">
                                        <h5>Alexander Volkov</h5>
                                    </div>
                                    <div class="col-8 text-muted text-right">
                                        19.07.2021
                                    </div>
                                </div>
                                <div class="row mt-1" style="padding-left: 15px; padding-right: 15px">
                                    <p> Увеличил производительность маленького собственного предприятия порядком на 40%, что также увеличило мою прибыль. Огромное спасибо ребята за низкие цены и за качественный продукт!</p>
                                </div>
                            </div>
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
            location.href = "/Basket/Payment";
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
