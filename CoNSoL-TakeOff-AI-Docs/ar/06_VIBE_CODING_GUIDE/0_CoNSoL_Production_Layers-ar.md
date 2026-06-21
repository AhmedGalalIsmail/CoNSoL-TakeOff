---
aliases:
  - 🧱 CoNSoL Production Layers
doc_id: 600
status: active
audience: AI coding agents + solo developer
---
# دليل طبقات الإنتاج — CoNSoL-TakeOff  
 
> **الغرض:** هذه هي نقطة الدخول إلى برمجة Vibe.
> يقوم وكيل الذكاء الاصطناعي المُضاف إلى هذا المشروع بقراءة هذا المستند أولاً، ثم يختار مهمة من ملف `1_Task_Backlog.md`.
> جميع تعريفات الطبقات خاصة بـ CoNSoL-TakeOff — لا توجد بنية تحتية عامة لا تنطبق.


---

## كيفية استخدام هذه الوثيقة

1. **التمهيد:** اقرأ خريطة الطبقات أدناه (دقيقتان)
2. **اختر طبقة** تتناسب مع مساحة العمل الحالية
3. **اقرأ قسم الطبقة** - يوضح لك الغرض، وما هو موجود بالفعل، وما هو مفقود، وما هو المطلوب إنتاجه
4. **راجع** معرّفات دورة حياة تطوير البرمجيات (SDLC) المدرجة - ابحث عنها في `05_Mega-File.md` إذا كنت بحاجة إلى تفاصيل كاملة
5. **اختر مهمة** من `1_Task_Backlog.md` ونفّذها

---

## خريطة الطبقات

```
ConSoL-TakeOff طبقات إنتاج 
│
├── L01 لوحة الرسم ومحرك الرسم ← السطح المرئي الأساسي
├── L02 منطق الأعمال والحساب ← الكميات والتكاليف والصيغ
├── L03 نموذج البيانات والاستمرارية ← الكيانات، قاعدة البيانات، JSON، الملفات
├── L04 واجهة المستخدم/تجربة المستخدم والعرض ← اللوحات، الأدوات، التفاعل
├── L05 بنية وجودة الكود ← الأنماط، حقن التبعية، المعايير
├── L06 الاختبار والتحقق ← اختبارات الوحدة، القبول، الأداء
├── L07 البناء، التغليف والنشر ← المُثبِّت، التكامل المستمر، الإصدار
└── L08 المراقبة والتسجيل ← التسجيل، تصحيح الأخطاء، التتبع
```

**غير مُضمَّن (لا ينطبق على WinForms v1 المستقل):**
البنية التحتية السحابية، شبكة توصيل المحتوى (CDN)، تحديد معدل الطلبات، التوسع الأفقي، التخزين المؤقت الموزع.

تظهر هذه العناصر مجددًا عند استهداف الوضع المتكامل أو المُستضاف على السحابة.

---

# لوحة الرسم ومحرك الرسم  — L01

## الغرض

تنفيذ لوحة الرسم ثنائية الأبعاد التي تستقبل مدخلات المستخدم، وتعرض الأشكال الهندسية، وتدير أنظمة الإحداثيات، وتتعامل مع جميع تفاعلات أدوات الرسم.
## مراجع دورة حياة تطوير البرمجيات

`FR-DT-001..FR-DT-052`, `FR-CV-001..FR-CV-007`, `0209 Canvas Engine Spec`, `UC-001`, `UC-010`, `UC-011`

## الموجود

- أدوات الخط، والمستطيل، والدائرة، والقطع الناقص، والخطوط المتعددة — تعمل
- التكبير (0.1x–10x)، والتحريك — يعمل
- عرض الشبكة، والتخزين المؤقت المزدوج — يعمل
- آلة حالة الأداة (أداة التحديد، أداة الخط، أداة المستطيل، أداة الدائرة، أداة التحريك) — تعمل
- تحديد الشكل مع تمييز مرئي — يعمل

## المفقود

| **الفجوة**     | **الوصف**                                                               |
| -------------- | ----------------------------------------------------------------------- |
| G-0301-06      | خوارزمية اختبار النقر لكل نوع شكل غير موثقة أو مُطبقة                   |
| G-0301-07      | منطق تحديد النافذة مقابل التحديد المتقاطع غير موثق                      |
| G-0301-08      | تحويل الإحداثيات الفيزيائية إلى المنطقية غير مُفعّل (الصيغة فقط موجودة) |
| G-0209-02      | مواصفات محرك Canvas غير مرتبطة بمتطلبات الوظائف الرسمية في SRS          |
| G-0209-03      | لم يتم تحديد مُشغّل تكبير/تصغير عجلة الماوس                             |
| FR-CV-010..030 | متطلبات وظائف Canvas مفقودة من SRS (الفجوة G-0104-02)                   |

## مهمة وكيل الذكاء الاصطناعي

تنفيذ إمكانيات لوحة الرسم المفقودة وفقًا للأولوية التالية:

1. تحويل الإحداثيات (من البكسل الفعلي إلى الوحدات المنطقية باستخدام عامل التكبير/التصغير)
2. اختبار النقر لكل نوع شكل (نقطة داخل مستطيل، نقطة على خط مع هامش خطأ، نقطة داخل دائرة)
3. تحديد النافذة مقابل التحديد المتقاطع (من اليسار إلى اليمين = نافذة/داخل بالكامل؛ من اليمين إلى اليسار = متقاطع/يتقاطع)
4. ربط تكبير/تصغير عجلة الماوس

## المخرجات المطلوبة

- خدمة `CoordinateConverter` (من الفعلي إلى المنطقي، تم اختبارها)
- تنفيذ اختبار النقر لكل نوع شكل في التسلسل الهرمي لفئة Shape الأساسية
- توثيق منطق وضع التحديد وبرمجته
- تسجيل جميع متطلبات الوظائف الجديدة في SRS §5.2 (من FR-CV-010 فصاعدًا)

## قواعد التحقق

- اختبار التحويل ذهابًا وإيابًا للإحداثيات: تحويل النقطة إلى منطقية، ثم التحويل مرة أخرى، الفرق < 0.001 وحدة
- اختبار النقر: النقر يُرجع الخطأ عند تجاوز حدود الشكل بمقدار 1 بكسل.
- تحديد النافذة: الشكل الموجود جزئيًا خارج النافذة = غير محدد.
- تحديد التقاطع: الشكل الموجود جزئيًا داخل منطقة التقاطع = محدد.
- التكبير/التصغير: تبقى الإحداثيات المنطقية ثابتة بعد التكبير/التصغير.

## أنماط غير مرغوب فيها

- دمج الإحداثيات المنطقية والفيزيائية في نفس المتغير.
- اختبار الاصطدام الذي يتحقق فقط من المربع المحيط (يفشل مع الخط والدائرة، إلخ).
- إعادة رسم اللوحة بالكامل لتغيير عنصر واحد (استخدام إبطال المنطقة المتسخة).

---
# منطق الأعمال والحسابات —  L02

## الهدف

تنفيذ محرك حساب الكميات: استخراج الكميات من الشكل الهندسي، تطبيق الصيغ، تجميع التكاليف، والإخراج الإجمالي. 
## مراجع دورة حياة تطوير البرمجيات

`FR-DT-040..FR-DT-052`, `UC-003`, `UC-004`, `UC-014`, `020103 §3 نموذج الأبعاد`, `0201 §6 بنية الحساب`

## الموجود

- فئة `TakeOffCalculator` — هيكل أساسي فقط، لم يتم تنفيذ `Calculate()`
- هيكل أساسي فقط  — `TakeOffService`، `MaterialService` 
- كيان `BusinessDefinition` — حقول المادة والكمية والتسعير موجودة
- تعداد وضع الأبعاد (D0/D1/D2/D3) — مُعرّف

## المفقود

| **العنصر**  | **الوصف**                                                                               |
| ----------- | --------------------------------------------------------------------------------------- |
| BUS-004     | `Calculator.Calculate()` — التوزيع الأساسي حسب وضع الأبعاد                              |
| BUS-005/006 | استخراج الأبعاد: D1 من طول الخط، D2 من العرض × الارتفاع، D3 من الارتفاع × العرض × الطول |
| BUS-007     | حل المعادلة: تطبيق تعبير `FormulaCode` على الكمية المحسوبة                              |
| BUS-008     | تجميع التكلفة: الكمية × سعر الوحدة لكل مادة                                             |
| BUS-009     | Ne                                                                                      |

طرح الكائن الفرعي: طرح مساحة/حجم الكائن الفرعي من الكائن الرئيسي |

| UC-003 | محرك العلامات الذكية (تعريف العلامة، وربطها بالكائن، وتعيين القيمة، والتجميع) |

| UC-004 | ملخص الاستخراج: التجميع حسب الطبقة/النوع/العلامة، والتصدير إلى CSV/XLSX |

## مهمة وكيل الذكاء الاصطناعي

نفّذ بالترتيب التالي:

1. ء `Calculator.Calculate(shape, dimensionMode)` — تفعيل D0/D1/D2/D3
2. اجتياز الكائنات المتداخلة — طرح كميات العناصر الفرعية من العنصر الرئيسي
3. مُقيّم تعبير الصيغة — مقارنة `FormulaCode` بالقيمة المحسوبة
4. ء `TakeOffService.GetSummary()` — تجميع البيانات حسب المجموعة، وإرجاع صفوف الجدول
5. التصدير إلى CSV (UC-014) — مفصول بعلامات جدولة، صف رأس، صف واحد لكل مجموعة

## المخرجات المطلوبة

- اجتياز `Calculator.Calculate()` لاختبارات الوحدة لجميع أوضاع الأبعاد الأربعة
- اختبار طرح الكائنات المتداخلة باستخدام مثال باب في الحائط
- إرجاع `TakeOffService.GetSummary()` جدولًا مُجمّعًا صحيحًا
- تصدير ملف CSV ينتج ملفًا صالحًا
- ربط جميع العناصر المُنفذة بمتطلبات الوظائف في RTM

## التحقق القواعد

- نتيجة D0 = عدد العناصر (عدد صحيح، ≥ 1)
- نتيجة D1 = طول الخط بالوحدات المنطقية (يطابق الشكل الهندسي)
- نتيجة D2 = العرض × الارتفاع، مع طرح قيمة الباب إذا كان متداخلًا
- نتيجة D3 = الارتفاع × العرض × الطول من خصائص Logical3D
- التكلفة = الكمية × سعر الوحدة، مقربة إلى منزلتين عشريتين
- ملف التصدير: UTF-8، مفصول بفواصل، الصف الأول = العناوين

## أنماط غير مرغوب فيها

- تضمين تعابير الصيغ في دالة Calculate() - يجب استخراجها من FormulaCode
- حساب التكلفة داخل محرك الرسم (ينتمي إلى طبقة التطبيق فقط)
- تعديل كائن الرسم أثناء الحساب (الحساب للقراءة فقط)

---
# - نموذج البيانات واستمراريتها —  L03 

## الغرض

تحديد وتنفيذ جميع هياكل البيانات، ومخطط قاعدة البيانات، وتنسيق التسلسل، واستمرارية الملفات لـ بيانات الرسم والأعمال في CoNSoL-TakeOff.

## مراجع دورة حياة تطوير البرمجيات

`020103 نموذج البيانات`، `0005_Gap_Analysis.md §020103`، `UC-013`، `UC-008`

## الموجود

- مخطط علاقات الكيانات (ERD) ذو 25 كيانًا (Mermaid) - كامل وموثوق (انظر `0005_Gap_Analysis.md`)
- كيانات المجال موجودة: `CanvasElement`، `CanvasLayout`، `BusinessDefinition`، `BlockModels`، `ElementRelationship`
- تنسيق ملف `.takeoff` (تحويل JSON باستخدام Newtonsoft.Json) - يعمل
- تشفير/فك تشفير الملفات - يعمل
- محول SQLite (وضع مستقل) - جزئي

## المفقود

| **الفجوة**  | **الوصف**                                                                                |
| ----------- | ---------------------------------------------------------------------------------------- |
| G-020103-02 | لا يوجد مخطط JSON رسمي (مسودة 07 أو OpenAPI) لكل كيان                                    |
| G-020103-03 | لا توجد استراتيجية لترحيل قاعدة البيانات (إصدار المخطط)                                  |
| G-020103-04 | لا توجد استراتيجية لفهرسة قاعدة البيانات (أعمدة المفاتيح الخارجية، الاستعلامات المتكررة) |
| G-020103-05 | لا توجد بيانات أولية (طبقات افتراضية، تعريفات علامات افتراضية، رموز مضمنة)               |
| FND-003     | وحدة التحقق من صحة CanvasLayout غير مُفعّلة                                              |
| FND-007     | وحدة التحقق من صحة CanvasElement غير مُفعّلة                                             |
| FND-013     | لم يتم إنشاء كيان الطبقة في الكود                                                        |
| FND-014     | منطق إدارة الطبقات غير مُفعّل                                                            |

## مهمة وكيل الذكاء الاصطناعي

1. إنشاء كيان `Layer` مطابق لمخطط علاقات الكيانات (ERD) (layer_id، canvas_id، name، visible، locked، printable، color، line_style، line_weight، sort_order)
2. إنشاء كيان `LayerGroup`
3. تنفيذ `CanvasLayoutValidator` و`CanvasElementValidator`
4. تعريف فهارس قاعدة البيانات: جميع أعمدة المفاتيح الخارجية + `object_type`، `layer_id` على DRAWING_OBJECT
5. تعريف مجموعة بيانات أولية: طبقة افتراضية واحدة ("Layer 1")، قيم التكوين الافتراضية
6. تحديد منهجية الترحيل (باستخدام حقل الإصدار في رأس JSON `.takeoff`)

## المخرجات المطلوبة

- كيانات `Layer.vb` و`LayerGroup.vb`، مطابقة تمامًا لمخطط علاقات الكيانات (ERD)
- وحدات التحقق مع اختبارات الوحدة
- تعريفات الفهارس موثقة في `020103`
- قائمة بيانات البداية موثقة في `020103 §5 الملحق`

## قواعد التحقق

- يجب أن يحتوي كيان الطبقة على جميع الحقول الموجودة في مخطط علاقات الكيانات (ERD) - لا يُسمح بإضافة أي حقول دون تحديث مخطط علاقات الكيانات أولاً
- التحقق من صحة CanvasLayout: scale_factor > 0، والوحدة ضمن مجموعة تعداد صالحة
- التحقق من صحة CanvasElement: object_type ضمن مجموعة تعداد صالحة، ويجب أن يشير layer_id إلى طبقة موجودة
- اختبار بيانات البداية: يحتوي المشروع الجديد دائمًا على طبقة واحدة على الأقل باسم "الطبقة 1"

## أنماط غير مرغوب فيها

- إضافة حقول إلى كيانات المجال غير الموجودة في مخطط علاقات الكيانات دون تحديث 020103
- تجاوز واجهة المستودع باستخدام استدعاءات مباشرة لقاعدة البيانات
- تخزين البيانات الهندسية في نفس ملف JSON blob مع بيانات العمل (فهي منفصلة حسب التصميم)

---
# واجهة المستخدم/تجربة المستخدم والعرض —  L04

## الغرض

تنفيذ طبقة العرض في WinForms: عنصر تحكم اللوحة، لوحة الخصائص، لوحة الطبقات، أشرطة الأدوات، القوائم، والنوافذ الحوارية — وفقًا لمواصفات تجربة المستخدم في `0208`.

## مراجع دورة حياة تطوير البرمجيات

`0208 تصميم تجربة المستخدم وواجهة المستخدم`، `UC-001..UC-008`، `FR-PP-001..FR-PP-008`، `FR-LP-001..FR-LP-004`، `FR-UI-020..FR-UI-023`
## العناصر الموجودة

- النافذة الرئيسية، الهيكل الأساسي — `MainForm` 
- سطح الرسم التفاعلي (يعمل) —  `CanvasControl` 
- موجود ولكنه غير مرتبط بحالة التحديد — `PropertiesPanel` 
- مربع حوار الكتل/المواد (موجود) — `BlockAssignmentForm` 
- إدارة المواد (موجود) — `MaterialCrudForm`
- واجهة مستخدم اختيار الأدوات — تعمل

## العناصر المفقودة

| **العنصر**                                      | **مرجع دورة حياة تطوير البرمجيات** | **الجهد**  |
| ----------------------------------------------- | ---------------------------------- | ---------- |
| لوحة الطبقات (`LayerPanel`)                     | UC-002، UC-007، FR-LP-001          | 8 ساعات    |
| لوحة الخصائص متصلة بالتحديد                     | UC-006، FR-PP-001                  | 3 ساعات    |
| حساس للسياقالنوع: لا شيء/مفرد/متعدد/أداة        | FR-PP-001، 0208 §4.1               | ساعتان     |
| عنصر نائب `(مختلط)` للقيم المختلفة              | FR-PP-004                          | ساعة واحدة |
| حقول ثلاثية الأبعاد منطقية في لوحة الخصائص      | FR-PP-008                          | ساعتان     |
| تخطيط صندوق الأدوات + التجميع                   | G-0208-08                          | ساعتان     |
| شريط الحالة                                     | G-0208-10                          | ساعتان     |
| عناصر القائمة الرئيسية (ملف، تحرير، عرض، أدوات) | IGN-012/013                        | 4 ساعات    |
| خريطة اختصارات لوحة المفاتيح                    | G-0208-06                          | ساعتان     |

## مهمة وكيل الذكاء الاصطناعي

نفّذ بالترتيب التالي:

1. عنصر تحكم `LayerPanel`: قائمة الطبقات، تبديل الرؤية، تبديل القفل، مؤشر الطبقة النشطة، زر الحذف الذي يُفعّل تدفق UC-007
2. ربط `PropertiesPanel` بحدث `CanvasControl.SelectionChanged`
3. حساسية السياق: تقرأ اللوحة نوع التحديد وتعرض الحقول الصحيحة
4. `(مختلط)` — عند وجود قيم مختلفة في التحديد المتعدد، اعرض نصًا توضيحيًا
5. شريط الحالة: إحداثيات المؤشر (منطقية)، اسم الطبقة النشطة، نسبة التكبير/التصغير، عدد العناصر

## المخرجات المطلوبة

- ملف `LayerPanel.vb` يُنفّذ FR-LP-001 إلى FR-LP-004
- ملف `PropertiesPanel.vb` يُطبّق حساسية السياق لجميع الحالات الخمس (جدول 0208 §4.1)
- شريط الحالة الذي يُظهر موضع المؤشر منطقيًا الوحدات
- جميع عناصر القائمة موصولة أو مُهيأة (لا توجد قوائم معطلة)

## قواعد التحقق

- لوحة الطبقات: حذف الطبقة النشطة يُظهر خطأً، ولا يحذفها
- لوحة الطبقات: حذف آخر طبقة يُظهر خطأً، ولا يحذفها
- لوحة الخصائص: تحديد 0 عنصر يُظهر خصائص اللوحة
- لوحة الخصائص: تحديد عنصر واحد يُظهر جميع خصائص هذا النوع
- لوحة الخصائص: تحديد أنواع مختلطة يُظهر الخصائص العامة فقط
- حقل `(مختلط)`: تعديله يُطبق القيمة الجديدة على جميع العناصر المحددة

## أنماط البرمجة السيئة

- منطق الأعمال داخل معالجات أحداث النموذج/عنصر التحكم (التفويض إلى طبقة التطبيق)
- استدعاءات قاعدة البيانات المباشرة من واجهة المستخدم (جميع عمليات الحفظ تمر عبر المستودع)
- تحديثات واجهة المستخدم غير الآمنة للخيوط (استخدم `Control.Invoke()` للعمليات غير المتزامنة)

---
# بنية وجودة الكود — L05 

## الغرض

الحفاظ على بنية الطبقات، وحقن التبعية، ومعايير البرمجة وأنماط تصميم تحافظ على قابلية صيانة قاعدة التعليمات البرمجية مع نموها.

## مراجع دورة حياة تطوير البرمجيات

`0201 وثائق التصميم`، `0301 وثائق التطوير`، `0205 ADRs`

## الموجود

- بنية رباعية الطبقات (المجال / التطبيق / البنية التحتية / سطح المكتب) - مُطبقة
- حاوية حقن التبعية (`Microsoft.Extensions.DependencyInjection`) - تعمل
- تطبيق الطبقات (لا توجد استدعاءات مباشرة لقاعدة البيانات من واجهة المستخدم)
- تسلسل JSON (`Newtonsoft.Json`) - يعمل
- واجهة `ILogger` - مُعرّفة (غير مستخدمة على نطاق واسع حتى الآن)

## المفقود

| **الفجوة** | **الوصف**                                                                                            |
| ---------- | ---------------------------------------------------------------------------------------------------- |
| G-0301-09  | نمط الأمر (ICommand مع تنفيذ/تراجع/إعادة) غير موثق                                                   |
| G-0302-01  | لم يتم تعريف IDrawingEngine وILayerService وITagService وITakeOffService                             |
| G-0301-10  | لم يتم تعريف استراتيجية معالجة الأخطاء: أخطاء اللوحة مقابل أخطاء البيانات مقابل أخطاء واجهة المستخدم |
| G-0205-01  | لم يتم إنشاء أي تقارير تصحيح الأخطاء بعد                                                             |
| G-0301-11  | لم يتم تحديد أهداف تغطية اختبار الوحدة لكل طبقة                                                      |

## مهمة وكيل الذكاء الاصطناعي

1. تعريف واجهة `ICommand` مع الدوال `Execute()` و`Undo()` و`Redo()` و`Description As String`
2. تنفيذ `CommandHistory` (مبني على بنية مكدسة، ومحدود بعدد N من المدخلات لكل `UNDO_STACK` في تصميم ERD)
3. تعريف واجهات الخدمات: `IDrawingEngine` و`ILayerService` و`ITagService` و`ITakeOffService`
4. تسجيل جميع الخدمات في `CompositionRoot.vb`
5. كتابة ADR-001: اختيار WinForms بدلاً من WPF للإصدار 1
6. كتابة ADR-002: استخدام SQLite للوضع المستقل

## المخرجات المطلوبة

- واجهة `ICommand.vb` + `CommandHistory.vb`
- `AddShapeCommand.vb` كأول تطبيق عملي (يغطي جميع المتطلبات) (تراجع UC-001)
- تم تعريف جميع واجهات الخدمة الأربع (حتى وإن لم يتم تنفيذها بالكامل)
- ADR-001 و ADR-002 في `05_SDLC_Library/02_Design/0205_ADR.md`

## قواعد التحقق

- التنفيذ ← التراجع ← الحالة تساوي حالة ما قبل التنفيذ (تم اختباره)
- التنفيذ ← التراجع ← الإعادة ← الحالة تساوي حالة ما بعد التنفيذ (تم اختباره)
- سجل الأوامر محدود: إضافة ما يتجاوز الحد يؤدي إلى حذف أقدم إدخال
- لا توجد تبعيات دائرية بين الطبقات (لا يحتوي المجال على أي مراجع إلى التطبيق، أو البنية التحتية، أو سطح المكتب)

## أنماط مضادة

- فئات شاملة (فئات تحتوي على أكثر من 300 سطر أو أكثر من 10 دوال عامة)
- حالة Singleton مشتركة بين الطبقات بدلاً من حقن التبعية
- التقاط الاستثناءات وتجاهلها دون تسجيل

---
#  الاختبار والتحقق — L06 

## الهدف

بناء وصيانة تغطية الاختبار لضمان صحة محرك الحساب، ومنطق المجال، وتدفقات حالات الاستخدام.
## مراجع دورة حياة تطوير البرمجيات

`0401 وثائق الاختبار`، من `G-0401-01 إلى G-0401-07`
## الموجود

- تمت الإشارة إلى NUnit كإطار عمل اختبار مخطط له
- لا توجد مشاريع اختبار حتى الآن
## المفقود

| **الفجوة** | **الوصف**                                                      | **الخطورة** |
| ---------- | -------------------------------------------------------------- | ----------- |
| G-0401-01  | لم يتم إعداد استراتيجية الاختبار                               | 🔴          |
| G-0401-02  | لا توجد حالات اختبار لـ FR-DT-xxx                              | 🔴          |
| G-0401-03  | لا توجد حالات اختبار لـ FR-CV-xxx                              | 🔴          |
| G-0401-04  | لا توجد حالات اختبار سلبية لقواعد التحقق                       | 🟠          |
| G-0401-05  | لا توجد اختبارات قبول لكل وحدة تحكم                            | 🟠          |
| G-0401-06  | لا توجد خطة لاختبار الأداء (NFR-001: إعادة رسم <16 مللي ثانية) | 🟡          |

## مهمة وكيل الذكاء الاصطناعي

1. إنشاء 4 مشاريع اختبار: `Domain.Tests`، `Application.Tests`، `Infrastructure.Tests`، `Desktop.Tests`
2. كتابة اختبارات الوحدة لـأو `Calculator.Calculate()` — جميع أوضاع الأبعاد الأربعة + الطرح المتداخل
3. كتابة اختبارات وحدة لـ `CoordinateConverter` — دقة التحويل ذهابًا وإيابًا
4. كتابة اختبارات وحدة لـ `CanvasLayoutValidator` و `CanvasElementValidator`
5. كتابة اختبار قبول لـ UC-001: محاكاة أحداث الماوس، والتأكد من أن كائن الخط في حالة الرسم
6. كتابة اختبار قبول لـ UC-004: تحميل التركيبة بثلاثة كائنات، وتشغيل عملية الاستخراج، والتأكد من الإخراج

## أهداف التغطية (من دقة G-0301-11)

| **الطبقة**                  | **الهدف** |
| --------------------------- | --------- |
| المجال                      | 80%       |
| التطبيق                     | 70%       |
| البنية التحتية              | 60%       |
| سطح المكتب (واجهة المستخدم) | 40%       |

## قواعد التحقق

- اجتياز جميع الاختبارات على `dotnet test` قبل دمج أي طلب سحب
- لا يجوز لأي اختبار الوصول إلى نظام الملفات الحقيقي (استخدم مجلدات مؤقتة أو نماذج محاكاة)
- أسماء طرق الاختبار: `MethodName_Scenario_ExpectedResult`
- يجب أن يحتوي كل طلب وحدة على اختبار قبول واحد على الأقل قبل وضع علامة "مكتمل" عليه

## أنماط مضادة

- الاختبارات التي تختبر المسار الصحيح فقط (اكتب حالة سلبية واحدة على الأقل لكل طريقة)
- الاختبارات التي تعتمد على ترتيب تنفيذ بعضها البعض
- التحقق من التساوي التام للأعداد العشرية (استخدم التسامح: `Math.Abs(actual - expected) < 0.001`)

---
# البناء والتغليف والنشر — L07  

## الغرض

تحديد مسار البناء، وتنسيق التغليف، واستراتيجية التثبيت لنشر Windows المستقل.
## مراجع دورة حياة تطوير البرمجيات

وثائق النشر `0304 DevSecOps & CI/CD`، `0501  `OQ-NEW-03 (تنسيق المُثبِّت)`,  

## الموجود

- يتم بناء المشروع باستخدام `dotnet build` (بدون أخطاء)
- ينتج الأمر `dotnet publish -c Release -r win-x64 --self-contained` ملفًا تنفيذيًا

## المفقود

| **الفجوة** | **الوصف**                                                     |
| ---------- | ------------------------------------------------------------- |
| G-0304-01  | مسار CI/CD غير مُعرَّف                                        |
| G-0501-01  | دليل تشغيل النشر غير مُعدّ                                    |
| OQ-NEW-03  | تنسيق المُثبِّت غير مُحدَّد (.msi مقابل MSIX مقابل ClickOnce) |
| G-0305-01  | استراتيجية البيئة (التطوير/ضمان الجودة/الإنتاج) غير مُعرَّفة  |


## مهمة وكيل الذكاء الاصطناعي

1. حل مشكلة OQ-NEW-03: التوصية بصيغة المثبّت مع شرح الأسباب (مرشح ADR)
2. تحديد مراحل مسار البناء: استعادة ← بناء ← اختبار ← نشر ← تغليف
3. كتابة سكربت بناء PowerShell (`build.ps1`) - آمن للاستخدام مع ASCII، بدون أحرف Unicode
4. كتابة دليل تشغيل النشر: خطوات التثبيت للمستخدم النهائي (غير التقني)
5. تحديد 3 بيئات: التطوير (محلي)، ضمان الجودة (جهاز اختبار)، الإنتاج (تثبيت العميل)

## المخرجات المطلوبة

- سكربت `build.ps1` الذي يُشغّل استعادة ← بناء ← اختبار ← نشر
- ملف `0501_Deployment_Documentation.md` يحتوي على تعليمات التثبيت خطوة بخطوة
- قرار صيغة المثبّت مُسجّل كـ ADR-003
- جدول مصفوفة البيئة في ملف `0305_Environment_Strategy.md`

## قواعد التحقق

- يُنهي البرنامج النصي `build.ps1` عمله بنجاح (الرمز 0)، وبفشل (الرمز غير الصفري).
- البرنامج النصي للبناء متكرر (يُنفذ مرتين = نفس النتيجة).
- تم اختبار المُثبِّت على جهاز ويندوز جديد بدون تثبيت .NET مُسبقًا.
- نص PowerShell النصي: يدعم ASCII فقط، بدون أسهم Unicode أو رموز رسم المربعات.

---
#  المراقبة والتسجيل — L08 

## الغرض

تزويد التطبيق بتسجيل استراتيجي لتمكين تصحيح الأخطاء، ومراقبة الأداء، وتتبع المشكلات.

## مراجع دورة حياة تطوير البرمجيات

`0606 قابلية المراقبة`، `0301 §13 التسجيل والتصحيح`
## الموجود

- واجهة `ILogger` - مُعرّفة
- مُنفّذة - `FileLogger` 
- التسجيل موجود: تغطية 5% تقريبًا (أدنى حد)

## المفقود

- نقاط تسجيل استراتيجية عبر الطبقات الأربع
- توقيت الأداء حول دورة إعادة الرسم (NFR-001: أقل من 16 مللي ثانية)
- تسجيل حدود الخطأ (الاستثناءات التي تم رصدها وتسجيلها مع السياق)
- توحيد تنسيق السجل (الطابع الزمني، المستوى، الطبقة، الرسالة)

## مهمة وكيل الذكاء الاصطناعي

1. تعريف تنسيق السجل: `[YYYY-MM-DD HH:mm:ss.fff] [LEVEL] [LAYER] [CLASS.Method] message`
2. إضافة `Logger.LogInfo()` عند: تفعيل الأداة، تثبيت الشكل، عملية الطبقة، الملف حفظ/تحميل، تشغيل الآلة الحاسبة
3. إضافة `Logger.LogDebug()` عند: تحويل الإحداثيات، استدعاءات HitTest، بداية/نهاية دورة العرض
4. إضافة `Logger.LogError()` عند: جميع الاستثناءات التي تم رصدها، مع تتبع المكدس والسياق
5. إضافة توقيت الأداء: تغليف `CanvasControl.OnPaint()` بساعة توقيت، وتسجيل إذا تجاوزت المدة 16 مللي ثانية

## المخرجات المطلوبة

- مواصفات تنسيق السجل موثقة في `0606_Observability.md`
- إضافة التسجيل إلى: `LineTool.OnMouseUp`، `Calculator.Calculate`، `TakeOffFileStore.Save`، `TakeOffFileStore.Load` على الأقل
- سجل الأداء: أي عملية عرض تتجاوز 16 مللي ثانية تُصدر إدخال `LogWarning`
- موقع ملف السجل موثق في `AppConfig`

## قواعد التحقق

- تشغيل التطبيق، رسم 3 أشكال، حفظ الملف ← يحتوي ملف السجل على 6 مدخلات معلومات على الأقل
- إحداث خطأ في التحقق ← يحتوي ملف السجل على مدخل خطأ واحد من نوع استثناء
- ترميز ملف السجل: UTF-8، رسائل سجل آمنة لـ ASCII (لا توجد رموز تعبيرية في مخرجات السجل)

## أنماط مضادة

- التسجيل عند كل دالة جلب/تعيين للخاصية (مُسهب للغاية، يُؤثر سلبًا على الأداء)
- تسجيل البيانات الحساسة (مسارات الملفات مسموحة؛ محتوى المستخدم غير مسموح)
- استخدام `Console.WriteLine` بدلًا من `ILogger` في أي مكان في قاعدة التعليمات البرمجية

---
## طبقة إدخال الذكاء الاصطناعي - ترقية مسودة 0601

تُظهر هذه الطبقة العناصر المتبقية من فئة 0601 التي لا تزال بحاجة إلى الترقية إلى مكتبة دورة حياة تطوير البرمجيات (SDLC) وقائمة المهام المتراكمة.

| **الفئة**        | **العنصر**                                      | **الحالة** | **الموقع**                                                     | **ملاحظات**                                                                                             |
| ---------------- | ----------------------------------------------- | ---------- | -------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| البيانات         | جلسة الاستيراد + مصدر arحفظ البيانات            | مُخطط      | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md`          | مطلوب قبل أن تتمكن كائنات الذكاء الاصطناعي المقبولة من التواجد في نموذج الملف                           |
| الذكاء الاصطناعي | تخزين نتائج التعرف الضوئي على الأحرف            | مُخطط      | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md`          | الحفاظ على النص المستخرج قابلاً للمراجعة والتدقيق                                                       |
| الذكاء الاصطناعي | اكتشاف المرشحين الهندسيين                       | مُخطط      | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md`          | يُنتج مرشحين تمت مراجعتهم، وليس الكائنات النهائية                                                       |
| الذكاء الاصطناعي | سير عمل تأكيد المقياس                           | مُخطط      | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md`          | يجب على المستخدم تأكيد المقياس قبل التحويل                                                              |
| الذكاء الاصطناعي | التصنيف + الثقة + المراجعة                      | مُخطط      | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md`          | مطلوب لعملية القبول/الرفض/التعديل                                                                       |
| واجهة المستخدم   | سطح مراجعة الذكاء الاصطناعي في نموذج نظيف       | مُخطط له   | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md`          | إضافة طبقات/لوحات دون التأثير على نموذج التطوير                                                         |
| الاختبار         | تجهيزات إدخال الذكاء الاصطناعي واختبارات القبول | مُخطط له   | `1_Task_Backlog.md` / `0401`                                   | تغطية الإدخال -> المراجعة -> التصدير                                                                    |
| العمليات         | تتبع الذكاء الاصطناعي وخيار التغليف الاحتياطي   | مُخطط له   | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` / `0304` | التقاط السجلات واختيار استراتيجية التعرف الضوئي على الأحرف/التحقق من صحة البيانات غير المتصلة بالإنترنت |

# قائمة التحقق النهائية - بوابة جاهزية الإنتاج

## الأساسيات (L01–L04)


- [ ] تم تنفيذ واختبار تحويل إحداثيات اللوحة
- [ ] تم تنفيذ اختبار النقر لكل نوع شكل
- [ ] ء UC-001 من البداية إلى النهاية: الرسم ← الحفظ ← إعادة التحميل تُظهر الشكل الصحيح
- [ ] ء UC-004 من البداية إلى النهاية: رسم الكائنات ← تشغيل عملية حساب الكميات ← تصدير ملف CSV
- [ ] ء UC-002 من البداية إلى النهاية: تعيين كائن إلى طبقة، تبديل إظهار/قفل الطبقة
- [ ] ء UC-007 من البداية إلى النهاية: حذف طبقة، إعادة تعيين الكائنات
- [ ] تم تنفيذ لوحة الطبقات (FR-LP-001 إلى FR-LP-004)
- [ ] لوحة الخصائص حساسة للسياق (جدول 0208 §4.1)

## الجودة (L05–L06)

- [ ] تم تنفيذ نمط ICommand (UC-012) (عملية التراجع/الإعادة)
- [ ] اجتياز أكثر من 40 اختبار وحدة (طبقة المجال + طبقة التطبيق)
- [ ] اجتياز اختبار القبول لكل من UC-001 وUC-004
- [ ] لا توجد تبعيات دائرية بين الطبقات
- [ ] جميع واجهات برمجة التطبيقات العامة مزودة بتعليقات توثيق XML

## التسليم (L07–L08)

- [ ] تشغيل ملف build.ps1 بشكل سليم (استعادة ← بناء ← اختبار ← نشر)
- [ ] تم اختبار المثبت المستقل على جهاز Windows نظيف
- [ ] تم إنشاء ملف سجل عند التشغيل الأول
- [ ] الأداء: تم تسجيل وقت إعادة الرسم، في حدود 16 مللي ثانية لأقل من 100 عنصر
- [ ] تم إعداد ADR-001 وADR-002 وADR-003

---
> راجع `1_Task_Backlog.md` للاطلاع على قائمة المهام المحددة مع إمكانية تتبع UC/FR/GAP.
---

---

# CoNSoL-TakeOff — Production Layers Guide

> **Purpose:** This is the vibe-coding entry point.
> An AI agent dropped into this project reads this doc first, then picks a task from `1_Task_Backlog.md`.
> All layer definitions are specific to CoNSoL-TakeOff — no generic infrastructure that doesn't apply.

---

## How to Use This Document

1. **Orient:** Read the layer map below (2 min)
2. **Pick a layer** matching the current work area
3. **Read the layer section** — it tells you purpose, what already exists, what is missing, and what to produce
4. **Cross-reference** the SDLC IDs listed — look them up in `05_Mega-File.md` if you need full detail
5. **Pick a task** from `1_Task_Backlog.md` and execute

---

## Layer Map

```
CoNSoL-TakeOff Production Layers
│
├── L01  Canvas & Drawing Engine          ← Core visual surface
├── L02  Business Logic & Calculation     ← Quantities, costs, formulas
├── L03  Data Model & Persistence         ← Entities, DB, JSON, files
├── L04  UI/UX & Presentation             ← Panels, tools, interaction
├── L05  Architecture & Code Quality      ← Patterns, DI, standards
├── L06  Testing & Verification           ← Unit tests, acceptance, perf
├── L07  Build, Package & Deployment      ← Installer, CI, release
└── L08  Observability & Logging          ← Logging, debugging, tracing
```

**Not included (not applicable to standalone WinForms v1):**
Cloud infrastructure, CDN, rate limiting, horizontal scaling, distributed caching.
These reappear if/when the Integrated or cloud-hosted mode becomes a target.

---
# L01 — Canvas & Drawing Engine

## Purpose

Implement the 2D visual canvas that accepts user input, renders geometry, manages coordinate systems, and handles all drawing tool interactions.

## SDLC References

`FR-DT-001..FR-DT-052`, `FR-CV-001..FR-CV-007`, `0209 Canvas Engine Spec`, `UC-001`, `UC-010`, `UC-011`

## What Exists

- Line, Rectangle, Circle, Ellipse, Polyline tools — working
- Zoom (0.1x–10x), pan — working
- Grid rendering, double-buffering — working
- Tool state machine (SelectTool, LineTool, RectangleTool, CircleTool, PanTool) — working
- Shape selection with visual highlight — working

## What Is Missing

| Gap | Description |
|---|---|
| G-0301-06 | HitTest algorithm per shape type not documented or implemented |
| G-0301-07 | Window vs crossing selection logic not documented |
| G-0301-08 | Physical↔logical coordinate conversion not implemented (only formula exists) |
| G-0209-02 | Canvas Engine Spec not linked to formal FRs in SRS |
| G-0209-03 | Mouse-wheel zoom trigger not specified |
| FR-CV-010..030 | Canvas FRs missing from SRS (gap G-0104-02) |

## AI Agent Mission

Implement missing canvas capabilities following this priority:

1. Coordinate conversion (physical px ↔ logical units using ScaleFactor)
2. HitTest per shape type (point-in-rect, point-on-line with tolerance, point-in-circle)
3. Window vs crossing selection (L→R = window/fully-inside; R→L = crossing/intersects)
4. Mouse-wheel zoom binding

## Required Deliverables

- `CoordinateConverter` service (physical ↔ logical, tested)
- HitTest implementation per shape type in the Shape base class hierarchy
- Selection mode logic documented and coded
- All new FRs registered in SRS §5.2 (FR-CV-010 onwards)

## Validation Rules

- Coordinate round-trip test: convert point to logical, convert back, delta < 0.001 units
- HitTest: clicking 1px outside a shape boundary returns false
- Window select: shape partially outside window = not selected
- Crossing select: shape partially inside crossing area = selected
- Zoom: logical coordinates unchanged after zoom in/out cycle

## Anti-Patterns

- Mixing logical and physical coordinates in the same variable
- HitTest that only checks bounding box (fails for Line, Circle, etc.)
- Redrawing the entire canvas for a single object change (use dirty-region invalidation)

---

---

# L02 — Business Logic & Calculation

## Purpose

Implement the take-off calculation engine: quantity extraction from geometry, formula application, cost rollup, and aggregated output.

## SDLC References

`FR-DT-040..FR-DT-052`, `UC-003`, `UC-004`, `UC-014`, `020103 §3 Dimension Model`, `0201 §6 Calculation Architecture`

## What Exists

- `TakeOffCalculator` class — skeleton only, `Calculate()` not implemented
- `TakeOffService`, `MaterialService` — skeleton only
- `BusinessDefinition` entity — material, quantity, pricing fields exist
- Dimension mode enum (D0/D1/D2/D3) — defined

## What Is Missing

| Item | Description |
|---|---|
| BUS-004 | `Calculator.Calculate()` — core dispatch by dimension mode |
| BUS-005/006 | Dimension extraction: D1 from line length, D2 from width×height, D3 from H×W×L |
| BUS-007 | Formula resolution: apply `FormulaCode` expression to calculated quantity |
| BUS-008 | Cost aggregation: quantity × unit_price per material |
| BUS-009 | Nested object subtraction: child object area/volume subtracted from parent |
| UC-003 | Smart Tag engine (define tag, attach to object, set value, aggregate) |
| UC-004 | Take-off summary: group-by layer/type/tag, export CSV/XLSX |

## AI Agent Mission

Implement in this order:

1. `Calculator.Calculate(shape, dimensionMode)` — switch on D0/D1/D2/D3
2. Nested object traversal — subtract child quantities from parent
3. Formula expression evaluator — resolve `FormulaCode` against calculated value
4. `TakeOffService.GetSummary()` — aggregate by group, return table rows
5. Export to CSV (UC-014) — tab-delimited, header row, one row per group

## Required Deliverables

- `Calculator.Calculate()` passing unit tests for all 4 dimension modes
- Nested object subtraction tested with door-in-wall example
- `TakeOffService.GetSummary()` returning correct aggregated table
- CSV export producing valid file
- All implemented items traced to FRs in the RTM

## Validation Rules

- D0 result = object count (integer, ≥ 1)
- D1 result = line length in logical units (matches geometry)
- D2 result = width × height, door subtracted if nested
- D3 result = H × W × L from Logical3D properties
- Cost = quantity × unit_price, rounded to 2 decimal places
- Export file: UTF-8, comma-delimited, first row = headers

## Anti-Patterns

- Hardcoding formula expressions in Calculate() — must resolve from FormulaCode lookup
- Calculating cost inside the drawing engine (belongs in Application layer only)
- Mutating the DrawingObject during calculation (calculation is read-only)

---

---

# L03 — Data Model & Persistence

## Purpose

Define and implement all data structures, database schema, serialization format, and file persistence for the CoNSoL-TakeOff drawing and business data.

## SDLC References

`020103 Data Model`, `0005_Gap_Analysis.md §020103`, `UC-013`, `UC-008`

## What Exists

- 25-entity ERD (Mermaid) — complete and authoritative (see `0005_Gap_Analysis.md`)
- `CanvasElement`, `CanvasLayout`, `BusinessDefinition`, `BlockModels`, `ElementRelationship` — Domain entities exist
- `.takeoff` file format (JSON serialization via Newtonsoft.Json) — working
- File encryption/decryption — working
- SQLite adapter (standalone mode) — partial

## What Is Missing

| Gap | Description |
|---|---|
| G-020103-02 | No formal JSON schema (draft-07 or OpenAPI) per entity |
| G-020103-03 | No DB migration strategy (schema versioning) |
| G-020103-04 | No DB index strategy (FK columns, frequent queries) |
| G-020103-05 | No seed data (default layers, default tag defs, built-in symbols) |
| FND-003 | CanvasLayout validation module not implemented |
| FND-007 | CanvasElement validation module not implemented |
| FND-013 | Layer entity not created in code |
| FND-014 | Layer management logic not implemented |

## AI Agent Mission

1. Create `Layer` entity matching ERD (layer_id, canvas_id, name, visible, locked, printable, color, line_style, line_weight, sort_order)
2. Create `LayerGroup` entity
3. Implement `CanvasLayoutValidator` and `CanvasElementValidator`
4. Define DB indexes: all FK columns + `object_type`, `layer_id` on DRAWING_OBJECT
5. Define seed data set: 1 default layer ("Layer 1"), default config values
6. Define migration approach (use a version field in the `.takeoff` JSON header)

## Required Deliverables

- `Layer.vb` and `LayerGroup.vb` entities, matching ERD exactly
- Validation modules with unit tests
- Index definitions documented in `020103`
- Seed data list documented in `020103 §5 Appendix`

## Validation Rules

- Layer entity must have exactly the fields in the ERD — no additions without updating the ERD first
- CanvasLayout validation: scale_factor > 0, unit in valid enum set
- CanvasElement validation: object_type in valid enum, layer_id must reference existing layer
- Seed data test: new project always has at least 1 layer named "Layer 1"

## Anti-Patterns

- Adding fields to Domain entities that aren't in the ERD without updating 020103
- Bypassing the Repository interface with direct DB calls
- Storing geometry in the same JSON blob as business data (they are separate by design)

---

---

# L04 — UI/UX & Presentation

## Purpose

Implement the WinForms presentation layer: canvas control, property panel, layer panel, toolbars, menus, and dialogs — following the UX spec in `0208`.

## SDLC References

`0208 UX & UI Design`, `UC-001..UC-008`, `FR-PP-001..FR-PP-008`, `FR-LP-001..FR-LP-004`, `FR-UI-020..FR-UI-023`

## What Exists

- `MainForm` — main window, skeleton
- `CanvasControl` — interactive drawing surface (working)
- `PropertiesPanel` — exists but not wired to selection state
- `BlockAssignmentForm` — block/material dialog (exists)
- `MaterialCrudForm` — material management (exists)
- Tool selection UI — working

## What Is Missing

| Item | SDLC Ref | Effort |
|---|---|---|
| Layer panel (`LayerPanel`) | UC-002, UC-007, FR-LP-001 | 8h |
| Property panel wired to selection | UC-006, FR-PP-001 | 3h |
| Context sensitivity: None/Single/Multi/Tool | FR-PP-001, 0208 §4.1 | 2h |
| `(mixed)` placeholder for differing values | FR-PP-004 | 1h |
| Logical 3D fields in property panel | FR-PP-008 | 2h |
| Toolbox layout + grouping | G-0208-08 | 2h |
| Status bar | G-0208-10 | 2h |
| Main menu items (File, Edit, View, Tools) | IGN-012/013 | 4h |
| Keyboard shortcuts map | G-0208-06 | 2h |

## AI Agent Mission

Implement in this order:

1. `LayerPanel` control: list of layers, visibility toggle, lock toggle, active layer indicator, Delete button triggering UC-007 flow
2. Wire `PropertiesPanel` to `CanvasControl.SelectionChanged` event
3. Context-sensitivity: panel reads selection type and shows correct fields
4. `(mixed)` — when multi-selection has differing values, show placeholder string
5. Status bar: cursor coordinates (logical), active layer name, zoom %, object count

## Required Deliverables

- `LayerPanel.vb` implementing FR-LP-001 through FR-LP-004
- `PropertiesPanel.vb` context-sensitivity for all 5 states (0208 §4.1 table)
- Status bar showing cursor position in logical units
- All menu items wired or stubbed (no dead menus)

## Validation Rules

- Layer panel: deleting the active layer shows error, does not delete
- Layer panel: deleting last layer shows error, does not delete
- Property panel: selecting 0 objects shows canvas properties
- Property panel: selecting 1 object shows all properties for that type
- Property panel: selecting mixed types shows universal properties only
- `(mixed)` field: editing it applies the new value to all selected objects

## Anti-Patterns

- Business logic inside Form/Control event handlers (delegate to Application layer)
- Direct DB calls from UI (all persistence goes through Repository)
- Thread-unsafe UI updates (use `Control.Invoke()` for async operations)

---

---

# L05 — Architecture & Code Quality

## Purpose

Maintain the layered architecture, dependency injection, coding standards, and design patterns that keep the codebase maintainable as it grows.

## SDLC References

`0201 Design Documentation`, `0301 Development Documentation`, `0205 ADRs`

## What Exists

- 4-layer architecture (Domain / Application / Infrastructure / Desktop) — implemented
- DI container (`Microsoft.Extensions.DependencyInjection`) — working
- Layering enforced (no direct DB calls from UI)
- JSON serialization (`Newtonsoft.Json`) — working
- `ILogger` interface — defined (not widely used yet)

## What Is Missing

| Gap | Description |
|---|---|
| G-0301-09 | Command pattern (ICommand with Execute/Undo/Redo) not documented |
| G-0302-01 | IDrawingEngine, ILayerService, ITagService, ITakeOffService not defined |
| G-0301-10 | Error handling strategy: canvas errors vs data errors vs UI errors not defined |
| G-0205-01 | No ADRs authored yet |
| G-0301-11 | No unit test coverage targets defined per layer |

## AI Agent Mission

1. Define `ICommand` interface with `Execute()`, `Undo()`, `Redo()`, `Description As String`
2. Implement `CommandHistory` (stack-based, bounded to N entries per `UNDO_STACK` ERD design)
3. Define service interfaces: `IDrawingEngine`, `ILayerService`, `ITagService`, `ITakeOffService`
4. Register all services in `CompositionRoot.vb`
5. Author ADR-001: choice of WinForms over WPF for v1
6. Author ADR-002: SQLite for standalone mode

## Required Deliverables

- `ICommand.vb` interface + `CommandHistory.vb`
- `AddShapeCommand.vb` as the first concrete implementation (covers UC-001 undo)
- All 4 service interfaces defined (even if not fully implemented)
- ADR-001 and ADR-002 in `05_SDLC_Library/02_Design/0205_ADR.md`

## Validation Rules

- Execute → Undo → state equals pre-Execute state (tested)
- Execute → Undo → Redo → state equals post-Execute state (tested)
- CommandHistory bounded: adding beyond limit drops oldest entry
- No circular dependencies between layers (Domain has zero references to Application, Infrastructure, Desktop)

## Anti-Patterns

- God classes (classes with >300 lines or >10 public methods)
- Singleton state shared across layers instead of DI
- Catching and swallowing exceptions without logging

---

---

# L06 — Testing & Verification

## Purpose

Build and maintain test coverage to ensure correctness of the calculation engine, domain logic, and use case flows.

## SDLC References

`0401 Testing Documentation`, `G-0401-01 through G-0401-07`

## What Exists

- NUnit referenced as planned testing framework
- No test projects exist yet

## What Is Missing

| Gap | Description | Severity |
|---|---|---|
| G-0401-01 | Test strategy not authored | 🔴 |
| G-0401-02 | No test cases for FR-DT-xxx | 🔴 |
| G-0401-03 | No test cases for FR-CV-xxx | 🔴 |
| G-0401-04 | No negative test cases for validation rules | 🟠 |
| G-0401-05 | No acceptance tests per UC | 🟠 |
| G-0401-06 | No performance test plan (NFR-001: <16ms redraw) | 🟡 |

## AI Agent Mission

1. Create 4 test projects: `Domain.Tests`, `Application.Tests`, `Infrastructure.Tests`, `Desktop.Tests`
2. Write unit tests for `Calculator.Calculate()` — all 4 dimension modes + nested subtraction
3. Write unit tests for `CoordinateConverter` — round-trip precision
4. Write unit tests for `CanvasLayoutValidator` and `CanvasElementValidator`
5. Write acceptance test for UC-001: simulate mouse events, assert Line object in drawing state
6. Write acceptance test for UC-004: load fixture with 3 objects, run take-off, assert output

## Coverage Targets (from G-0301-11 resolution)

| Layer | Target |
|---|---|
| Domain | 80% |
| Application | 70% |
| Infrastructure | 60% |
| Desktop (UI) | 40% |

## Validation Rules

- All tests pass on `dotnet test` before any PR is merged
- No test may touch the real file system (use temp folders or mocks)
- Test method names: `MethodName_Scenario_ExpectedResult`
- Each UC must have at least 1 acceptance test before that UC is marked Done

## Anti-Patterns

- Tests that only test the happy path (write at least 1 negative case per method)
- Tests that depend on each other's execution order
- Asserting exact floating-point equality (use tolerance: `Math.Abs(actual - expected) < 0.001`)

---

---

# L07 — Build, Package & Deployment

## Purpose

Define the build pipeline, packaging format, and installer strategy for standalone Windows deployment.

## SDLC References

`0304 DevSecOps & CI/CD`, `0501 Deployment Documentation`, `OQ-NEW-03 (installer format)`

## What Exists

- Project builds with `dotnet build` (no errors)
- `dotnet publish -c Release -r win-x64 --self-contained` produces executable

## What Is Missing

| Gap | Description |
|---|---|
| G-0304-01 | CI/CD pipeline not defined |
| G-0501-01 | Deployment runbook not authored |
| OQ-NEW-03 | Installer format undecided (.msi vs MSIX vs ClickOnce) |
| G-0305-01 | Environment strategy (Dev/QA/Prod) not defined |

## AI Agent Mission

1. Resolve OQ-NEW-03: recommend installer format with rationale (ADR candidate)
2. Define build pipeline stages: Restore → Build → Test → Publish → Package
3. Write PowerShell build script (`build.ps1`) — ASCII-safe, no Unicode box chars
4. Write deployment runbook: install steps for end user (non-technical)
5. Define 3 environments: Dev (local), QA (test machine), Prod (client install)

## Required Deliverables

- `build.ps1` script that runs Restore → Build → Test → Publish
- `0501_Deployment_Documentation.md` with step-by-step installer instructions
- Installer format decision recorded as ADR-003
- Environment matrix table in `0305_Environment_Strategy.md`

## Validation Rules

- `build.ps1` exits 0 on success, non-zero on any failure
- Build script is idempotent (run twice = same result)
- Installer tested on a clean Windows machine with no .NET pre-installed
- PowerShell script: ASCII-only, no Unicode arrows or box-drawing characters

---

---

# L08 — Observability & Logging

## Purpose

Instrument the application with strategic logging to enable debugging, performance monitoring, and issue tracing.

## SDLC References

`0606 Observability`, `0301 §13 Logging & Debugging`

## What Exists

- `ILogger` interface — defined
- `FileLogger` — implementation exists
- Logging present: ~5% coverage (minimal)

## What Is Missing

- Strategic logging points across all 4 layers
- Performance timing around redraw cycle (NFR-001: <16ms)
- Error boundary logging (exceptions caught and logged with context)
- Log format standardization (timestamp, level, layer, message)

## AI Agent Mission

1. Define log format: `[YYYY-MM-DD HH:mm:ss.fff] [LEVEL] [LAYER] [CLASS.Method] message`
2. Add `Logger.LogInfo()` at: tool activation, shape commit, layer operation, file save/load, calculator run
3. Add `Logger.LogDebug()` at: coordinate conversion, HitTest calls, render cycle start/end
4. Add `Logger.LogError()` at: all caught exceptions, with stack trace and context
5. Add performance timing: wrap `CanvasControl.OnPaint()` with stopwatch, log if > 16ms

## Required Deliverables

- Log format spec documented in `0606_Observability.md`
- Logging added to at least: `LineTool.OnMouseUp`, `Calculator.Calculate`, `TakeOffFileStore.Save`, `TakeOffFileStore.Load`
- Performance log: any render exceeding 16ms emits a `LogWarning` entry
- Log file location documented in `AppConfig`

## Validation Rules

- Run app, draw 3 shapes, save file → log file contains at least 6 Info entries
- Trigger a validation error → log file contains 1 Error entry with exception type
- Log file encoding: UTF-8, ASCII-safe log messages (no emoji in log output)

## Anti-Patterns

- Logging at every property getter/setter (too verbose, kills performance)
- Logging sensitive data (file paths are OK; user content is not)
- Using `Console.WriteLine` instead of `ILogger` anywhere in the codebase

---

---

## AI Intake Overlay - Draft 0601 Promotion

This overlay captures the remaining 0601 items that still need promotion into the live SDLC library and backlog.

| Category | Item | Status | Where It Belongs | Notes |
|---|---|---|---|---|
| Data | Import session + source artifact persistence | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` | Needed before accepted AI objects can live in the file model |
| AI | OCR result storage | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` | Keep extracted text reviewable and auditable |
| AI | Geometry candidate detection | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` | Produces reviewed candidates, not final objects |
| AI | Scale confirmation workflow | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` | User must confirm scale before conversion |
| AI | Classification + confidence + review | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` | Needed for accept/reject/edit flow |
| UI | AI review surface in the clean form | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` | Add overlays/panels without disturbing the dev form |
| Testing | AI intake fixtures and acceptance tests | Planned | `1_Task_Backlog.md` / `0401` | Cover intake -> review -> export |
| Ops | AI tracing and packaging fallback | Planned | `0_CoNSoL_Production_Layers.md` / `1_Task_Backlog.md` / `0304` | Capture logs and pick offline OCR/CV strategy |

# Final Checklist — Production Readiness Gate

## Core (L01–L04)


- [ ] Canvas coordinate conversion implemented and tested
- [ ] HitTest implemented per shape type
- [ ] UC-001 end-to-end: draw → save → reload shows correct shape
- [ ] UC-004 end-to-end: draw objects → run take-off → export CSV
- [ ] UC-002 end-to-end: assign object to layer, layer visible/locked toggle
- [ ] UC-007 end-to-end: delete layer, objects reassigned
- [ ] Layer panel implemented (FR-LP-001 through FR-LP-004)
- [ ] Property panel context-sensitive (0208 §4.1 table)

## Quality (L05–L06)


- [ ] ICommand pattern implemented (UC-012 Undo/Redo working)
- [ ] 40+ unit tests passing (Domain + Application layer)
- [ ] Acceptance test per UC-001 and UC-004 passing
- [ ] No circular layer dependencies
- [ ] All public APIs have XML doc comments

## Delivery (L07–L08)


- [ ] build.ps1 runs clean (Restore → Build → Test → Publish)
- [ ] Standalone installer tested on clean Windows machine
- [ ] Log file produced on first run
- [ ] Performance: redraw time logged, within 16ms for <100 objects
- [ ] ADR-001, ADR-002, ADR-003 authored

---
> See `1_Task_Backlog.md` for the concrete task list with UC/FR/GAP traceability.

