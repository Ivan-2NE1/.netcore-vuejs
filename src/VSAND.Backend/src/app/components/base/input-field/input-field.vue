<template>
    <div v-bind:class="['form-group', {'has-error': !valid, 'has-focus': hasFocus, 'input-lg': inputSize === 'lg', 'input-sm': inputSize === 'sm'}]">
        <label :class="['control-label', {'has-value': hasValue}]"
               :for="inputId === '' ? false : inputId" 
               v-if="label !== null && label !== ''">{{ label }}</label>
        <component v-bind:class="{'input-group': $slots['inputicons'] || $slots['inputbuttons'] || $slots['inputappend']}" v-bind:is="($slots['inputicons'] || $slots['inputbuttons'] || $slots['inputappend']) ? 'div' : 'span'">
            <input v-bind:type="inputType" class="form-control" ref="mdinputfld"
                   v-bind:id="inputId === '' ? false : inputId"
                   v-bind:name="inputId === '' ? false : inputId"
                   v-bind:placeholder="placeholder !== null && placeholder !== '' ? placeholder : ''"
                   v-bind:maxlength="maxLength > 0 ? maxLength : false"
                   v-bind:required="required"
                   v-model.trim="inputValue"
                   v-mask="inputMask"
                   v-on:change="inputChanged"
                   v-ascii
                   v-bind:disabled="inputDisabled"
                   @blur="validated = true; hasFocus = false"
                   @focus.native="onFocus"
                   @focus="onFocus">
            <div class="input-group-addon" v-if="$slots['inputicons']" key="inputIcons">
                <slot name="inputicons"></slot>
            </div>
            <span class="input-group-append" v-if="$slots['inputbuttons']" key="inputButtons">
                <slot name="inputbuttons"></slot>
            </span>
            <div class="input-group-append" v-if="$slots['inputappend']" key="inputAddOn">
                <slot name="inputappend"></slot>
            </div>
        </component>
        <div class="form-text text-muted" v-if="formHelp !== null && formHelp !== ''">{{ formHelp }}</div>
    </div>
</template>

<script>
    var Inputmask = require('inputmask');

    export default {
        name: "input-field",
        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-field-' + Math.random().toString(36).substr(2, 9)
                }
            },
            type: {
                type: String,
                default: "text",
                validator(value) {
                    // The value must match one of these strings
                    return ['text', 'password', 'color', 'date', 'datetime-local', 'email', 'month', 'number', 'range', 'search', 'tel', 'time', 'url', 'week'].indexOf(value) !== -1
                }
            },
            autocomplete: {
                type: String,
                default: "input-af-off" // default to disabling autofill as much as we can
            },
            defaultValue: {
                type: [String, Number],
                default: ""
            },
            label: {
                type: String,
                default: ""
            },
            placeholder: {
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
            validationRe: {
                type: String,
                default: ""
            },
            invalidMsg: {
                type: String,
                default: ""
            },
            formHelp: {
                type: String,
                default: ""
            },
            inputMask: {
                type: Object,
                default: null
            },
            loading: {
                type: Boolean,
                default: false
            },
            maxLength: {
                type: Number,
                default: null
            },
            inputSize: {
                type: String,
                default: "",
                validator(value) {
                    // The value must match one of these strings
                    return ['', 'sm', 'lg'].indexOf(value) !== -1
                }
            },
            inputDisabled: {
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                inputValue: this.defaultValue,
                inputType: this.type,
                validated: false,
                hasFocus: false,
                cLoading: this.loading,
                validationRegEx: this.validationRe,
                isExternalInvalid: this.externalInvalid,
            };
        },
        computed: {
            valid() {
                var bValidRe = true;
                if (this.validationRegEx !== "") {
                    var oRe = new RegExp(this.validationRegEx);
                    if (!oRe.test(this.inputValue)) {
                        bValidRe = false;
                    }
                }
                if (!this.validated) return true;
                if (this.externalInvalid !== null && this.externalInvalid) {
                    // double-check to see if we have an internal validator we can try
                    if (!bValidRe) {
                        return false;
                    }
                }
                if (!bValidRe) {
                    return false;
                }
                if (!this.required) return true;
                if (this.inputValue === null || this.inputValue === "") return false;
                return true;
            },
            hasValue() {
                return (this.inputValue !== null && this.inputValue !== "");
            },
            hasAddOnIconSlot() {
                return !!this.$slots.inputicons;
            },

            hasAddOnButtonSlot() {
                return !!this.$slots.inputbuttons;
            },
        },

        watch: {
            externalInvalid(newval) {
                this.isExternalInvalid = newval;
                this.validated = true;
            },
            inputValue(newval) {
                var emitVal = newval;
                if (this.maxLength !== null && this.maxLength > 0 && newval.length > this.maxLength) {
                    emitVal = emitVal.substring(0, this.maxLength);
                }
                this.$emit("input", emitVal);
            },
            defaultValue(newVal) {
                this.inputValue = newVal;
            }
        },

        directives: {
            mask: {
                // directive definition
                bind(el, binding) {
                    // make sure the binding params are valid-ish
                    if (binding.value !== null && binding.value !== "") {
                        var maskOpts = binding.value;
                        maskOpts.showMaskOnHover = false;
                        maskOpts.autoUnmask = true;
                        maskOpts.clearIncomplete = true;
                        Inputmask(maskOpts).mask(el);
                    }
                },
                unbind(el) {
                    Inputmask.remove(el);
                }
            },
            ascii: {
                bind(el, binding, vnode) {
                    var handler = function (e) {
                        var oldVal = e.target.value;
                        var newVal = oldVal.replace(/[^\x00-\x7F]/g, '');

                        if (oldVal !== newVal) {
                            // remove all non-ASCII characters and trigger an 'input' event
                            e.target.value = newVal;

                            if (typeof (Event) === 'function') {
                                var event = new Event('input');
                            }
                            else {
                                var event = document.createEvent('Event');
                                event.initEvent('input', true, true);
                            }

                            vnode.elm.dispatchEvent(event);
                        }
                    }

                    el.addEventListener('input', handler);
                }
            }
        },

        methods: {
            setFieldMask(sNewType) {
                var oInput = this.$refs["mdinputfld"];
                Vue.nextTick().then(function () {
                    oInput.type = sNewType;
                });
            },

            inputChanged(newVal) {
                this.$emit("change", this.inputValue);
            },

            onFocus() {
                this.hasFocus = true;
                this.$emit("focus")
            },
        }
    };
</script>
