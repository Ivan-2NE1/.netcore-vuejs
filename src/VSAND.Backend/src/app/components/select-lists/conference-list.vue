<template>
    <vue-selectbase v-bind:input-id="inputId"
                    v-bind:label="enableMultiple ? 'Conferences' : 'Conference'"
                    choose-prompt="Select a Conference"
                    v-bind:default-value="selected"
                    v-bind:enable-multiple="enableMultiple"
                    v-bind:required="required"
                    v-bind:external-invalid="externalInvalid"
                    v-bind:options="options"
                    option-label-key="name"
                    v-on:change="selected = $event"></vue-selectbase>
</template>

<script>
    import vueSelectBase from "../base/vue-selectbase/vue-selectbase.vue";

    export default {
        name: "conference-list",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        components: {
            "vue-selectbase": vueSelectBase
        },
        props: {
            inputId: {
                type: String,
                default: "",
            },
            defaultValue: {
                type: Array,
                default: null
            },
            required: {
                type: Boolean,
                default: false
            },
            externalInvalid: {
                type: Boolean,
                default: null
            },
            enableMultiple: {
                type: Boolean,
                default: false,
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                options: [],
                selected: [],
                validated: false,
                cLoading: false,

            }
        },

        created() {
            var vm = this;
            // load the list from the API endpoint
            $.get("/siteapi/teams/customcodelist?codeName=Conference", function (data) {
                vm.options = data;

                if (vm.defaultValue !== null && vm.defaultValue.length > 0) {
                    vm.selected = [];
                    for (var i = 0; i < vm.defaultValue.length; i++) {
                        var selId = vm.defaultValue[i];
                        // get the selected value and make it active
                        var selOpt = data.find(function (a) {
                            return a.id === selId;
                        });
                        if (selOpt !== null) {
                            vm.selected.push(selOpt);
                        }
                    }
                }
            });
        },

        watch: {
            selected(newval) {
                this.$emit('change', newval);
            }
        },
    };
</script>