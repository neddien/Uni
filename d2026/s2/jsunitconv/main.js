const UNITS = {
  length: {
    units: { 'km': 1000, 'm': 1, 'cm': 0.01, 'mm': 0.001, 'in': 0.0254, 'ft': 0.3048, 'yd': 0.9144, 'mi': 1609.344 },
  },
  mass: {
    units: { 'kg': 1000, 'g': 1, 'lb': 453.592 },
  },
  time: {
    units: { 'sec': 1e9, 'ms': 1e6, 'ns': 1 },
  },
  temperature: {
    special: true,
    units: ['°C', '°F', 'K'],
  },
};

function toC(val, unit) {
  if (unit === '°C') return val;
  if (unit === '°F') return (val - 32) * 5 / 9;
  if (unit === 'K')  return val - 273.15;
}

function fromC(celsius, unit) {
  if (unit === '°C') return celsius;
  if (unit === '°F') return celsius * 9 / 5 + 32;
  if (unit === 'K')  return celsius + 273.15;
}

function updateUnits() {
  const cat = document.getElementById('category').value;
  const fromSel = document.getElementById('from');
  const toSel = document.getElementById('to');

  fromSel.innerHTML = '';
  toSel.innerHTML = '';

  const catData = UNITS[cat];
  const list = catData.special ? catData.units : Object.keys(catData.units);

  list.forEach(u => {
    fromSel.add(new Option(u, u));
    toSel.add(new Option(u, u));
  });

  if (toSel.options.length > 1) toSel.selectedIndex = 1;

  convert();
}

function convert() {
  const val = parseFloat(document.getElementById('value').value);
  const cat = document.getElementById('category').value;
  const from = document.getElementById('from').value;
  const to = document.getElementById('to').value;
  const resultEl = document.getElementById('result');

  const catData = UNITS[cat];
  let result;

  if (catData.special) {
    result = fromC(toC(val, from), to);
  } else {
    result = val * catData.units[from] / catData.units[to];
  }

  resultEl.textContent = `${val} ${from} = ${result} ${to}`;
}

updateUnits();
