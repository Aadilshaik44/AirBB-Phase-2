// AirBB: auto-initialize every date-range input marked with .js-daterange
(function () {
  function initOne(el) {
    if (!$.fn || !$.fn.daterangepicker) return;       // plugin not loaded

    // Hidden fields to sync with (IDs passed via data-* attributes)
    const sId = el.dataset.startField;
    const eId = el.dataset.endField;
    const sEl = sId ? document.getElementById(sId) : null;
    const eEl = eId ? document.getElementById(eId) : null;

    const sVal = sEl && sEl.value ? sEl.value : "";
    const eVal = eEl && eEl.value ? eEl.value : "";

    // Prefill visible input if hidden fields already have values
    if (sVal && eVal && window.moment) {
      el.value = moment(sVal).format('MM/DD/YYYY') + ' - ' + moment(eVal).format('MM/DD/YYYY');
    }

    $(el).daterangepicker({
      autoUpdateInput: !!(sVal && eVal),
      locale: { cancelLabel: 'Clear' }
    });

    $(el).on('apply.daterangepicker', function (_ev, picker) {
      const s = picker.startDate.format('MM/DD/YYYY');
      const e = picker.endDate.format('MM/DD/YYYY');
      if (sEl) sEl.value = s;
      if (eEl) eEl.value = e;
      this.value = s + ' - ' + e;
    });

    $(el).on('cancel.daterangepicker', function () {
      this.value = '';
      if (sEl) sEl.value = '';
      if (eEl) eEl.value = '';
    });
  }

  function initAll() {
    document.querySelectorAll('.js-daterange').forEach(initOne);
  }

  // Initialize once DOM is ready
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initAll);
  } else {
    initAll();
  }
})();
