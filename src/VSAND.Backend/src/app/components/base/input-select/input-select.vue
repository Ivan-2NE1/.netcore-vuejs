<template>
    <div v-bind:class="['form-group', {'has-error': !valid}, {'mb-0': !padding}]">
        <label v-bind:class="['control-label', {'has-value': hasValue}]"
               v-bind:for="inputId"
               v-if="label !== null && label !== ''">{{ label }}</label>
        <select class="form-control" width="100%"
                v-bind:id="inputId === '' ? false : inputId"
                v-bind:name="inputId === '' ? false : inputId"
                v-model="selected"
                v-on:change="updateValue"
                v-bind:required="required">
            <option v-bind:disabled="required" v-bind:aria-disabled="required" :value="getDefaultValue()">{{ choosePrompt }}</option>
            <option v-for="option in adaptedOptions"
                    v-bind:key="option.key"
                    v-bind:value="option.value"
                    v-text="option.label" />
        </select>
    </div>
</template>

<script>
    export default {
        name: "input-select",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-select-' + Math.random().toString(36).substr(2, 9)
                }
            },
            optionAdapter: {
                type: Function,
                default(value) {
                    return {
                        label: value,
                        key: value,
                        value: value
                    };
                }
            },
            options: {
                type: Array,
                default: []
            },
            defaultValue: {
                type: [Array, String, Number, Object],
                default: null
            },
            label: {
                type: String,
                default: ""
            },
            required: {
                type: Boolean,
                default: false
            },
            externalInvalid: {
                type: Boolean,
                default: null
            },
            loading: {
                type: Boolean,
                default: false
            },
            choosePrompt: {
                type: String,
                default: "Choose"
            },
            padding: {
                type: Boolean,
                default: true
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                selected: this.computeSelectedValue(this.defaultValue),
                validated: false,
                cLoading: this.loading,
            };
        },
        computed: {
            adaptedOptions() {
                var vm = this;
                return this.options.map(function (x) {
                    return vm.optionAdapter(x);
                });
            },
            valid() {
                if (!this.validated) return true;
                if (this.externalInvalid !== null && this.externalInvalid) return false;
                if (!this.required) return true;
                if (this.selected === null || this.selected === "") return false;
                return true;
            },
            hasValue() {
                return (this.selected !== null && this.selected !== "");
            },
        },
        watch: {
            externalInvalid() {
                this.validated = true;
            },
            loading(newval) {
                this.cLoading = newval;
            },
            defaultValue(newval) {
                this.selected = this.computeSelectedValue(newval);
            },
        },
        methods: {
            getDefaultValue() {
                switch (typeof this.defaultValue) {
                    case "string":
                        return "";
                        break;
                    case "number":
                        return "";
                        break;
                    case "object":
                        return null;
                        break;
                    default:
                        return null;
                        break;
                }
            },
            computeSelectedValue(newval) {
                var vm = this;

                if (Array.isArray(newval)) {
                    return newval.map(function (x) {
                        return vm.optionAdapter(x).value;
                    });
                }
                else if (typeof (newval) === "object") {
                    return newval && vm.optionAdapter(newval).value;
                }

                return newval;
            },
            updateValue() {
                //const newValue = this.adaptedOptions.find(x => x.id === this.selected).value;

                var vm = this;
                var newValue = vm.adaptedOptions.find(function (x) {
                    return x.value === vm.selected;
                });

                // Emitting a `change` event with the new
                // value of the `<select>` field, updates
                // all values bound with `v-model`.
                this.$emit("change", newValue);
            },
        }
    };
</script>