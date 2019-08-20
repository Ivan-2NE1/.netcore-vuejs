<template>
    <div v-bind:class="['form-group', {'has-error': !valid, 'has-focus': hasFocus, 'input-lg': inputSize === 'lg', 'input-sm': inputSize === 'sm'}]">
        <label v-if="label !== null && label !== ''">{{ label }}</label>
        <div class="input-group" v-bind:id="'dpWrap-' + inputId" data-target-input="nearest">
            <div class="input-group-prepend" v-if="$slots['beforeinput']" key="beforeinputSlot">
                <slot name="beforeinput"></slot>
            </div>            
            <input type="text" class="form-control datetimepicker-input"
                   v-bind:id="inputId"
                   v-model:trim="inputValue"
                   v-bind:data-target="'#dpWrap-' + inputId"
                   v-on:focus.native="showDatetimePicker"
                   v-on:focus="showDatetimePicker" />
            <div class="input-group-append">
                <div class="input-group-text" v-bind:data-target="'#dpWrap-' + inputId" data-toggle="datetimepicker">
                    <i class="far fa fa-fw fa-calendar"></i>
                </div>
                <slot name="afterinput"></slot>
            </div>            
        </div>
    </div>
</template>

<script>
    import 'tempusdominus-bootstrap-4';

    const events = ['hide', 'show', 'change', 'error', 'update'];
    const eventNameSpace = 'datetimepicker';

    export default {
        name: "input-calendar",

        components: {

        },

        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-calendar-' + Math.random().toString(36).substr(2, 9)
                }
            },
            // http://eonasdan.github.io/bootstrap-datetimepicker/Options/
            config: {
                type: Object,
                default: () => ({
                    format: 'L'
                })
            },
            label: {
                type: String,
                default: ""
            },
            defaultValue: {
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
            inputSize: {
                type: String,
                default: "",
                validator(value) {
                    // The value must match one of these strings
                    return ['', 'sm', 'lg'].indexOf(value) !== -1
                }
            },
        },

        data() {
            return {
                dp: null,
                // jQuery DOM
                elem: null,
                inputValue: this.defaultValue,
                validated: false,
                hasFocus: false,
                showPicker: false,
            }
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
            },

            /**
           * Watch for any change in options and set them
           *
           * @param newConfig Object
           */
            config: {
                deep: true,
                handler(newConfig) {
                    this.dp && this.dp.options(newConfig);
                }
            }
        },

        mounted() {
            // Return early if date-picker is already loaded
            /* istanbul ignore if */
            if (this.dp) return;
            // Handle wrapped input
            this.elem = jQuery('#dpWrap-' + this.inputId);
            // Init date-picker
            this.elem.datetimepicker(this.config);
            // Store data control
            this.dp = this.elem.data('datetimepicker');
            // Set initial value
            // Date value coming in could be string or date object, so enforce formatting for calendar init
            this.dp.date(moment(String(this.inputValue)).format("MM/DD/YYYY"));
            // Watch for changes
            this.elem.on('change.datetimepicker', this.onChange);
            // Register remaining events
            this.registerEvents();
        },

        methods: {
            showDatetimePicker() {
                this.elem.datetimepicker("show");
            },
            onChange(event) {
                let formattedDate = event.date ? event.date.format(this.dp.format()) : null;
                this.inputValue = formattedDate;
            },
            /**
             * Emit all available events
             */
            registerEvents() {
                events.forEach((name) => {
                    this.elem.on(`${name}.${eventNameSpace}`, (...args) => {
                        this.$emit(`on-${name}`, ...args);
                    });
                })
            }
        },

        /** * Free up memory */
        beforeDestroy() {
            if (this.dp) {
                this.dp.destroy();
                this.dp = null;
                this.elem = null;
            }
        },
    };
</script>

<style lang="scss">
    @import '~tempusdominus-bootstrap-4/src/sass/tempusdominus-bootstrap-4-build';
</style>