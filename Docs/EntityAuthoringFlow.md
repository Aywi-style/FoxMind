# Entity Authoring Flow

Этот документ описывает текущий флоу сборки игровых сущностей: будущих врагов и нового главного героя. Документ нужно обновлять каждый раз, когда меняется набор gameplay-фич, ECS-компонентов или порядок настройки префабов.

## Общий принцип

Сущность в проекте собирается через `BaseEntityBaker`/наследника baker'а и набор `IEntityFeature` компонентов. В инспекторе должны висеть только те feature-компоненты, которые реально описывают свойства сущности или ссылки на MonoBehaviour-части. Чисто runtime-состояния и request-компоненты руками на префаб обычно не добавляются.

## Новый Главный Герой

Базовый вариант главного героя должен иметь:

- `PlayerControlledComp`: помечает сущность как управляемую игроком.
- `TransformComp`: ссылка на transform сущности.
- `UnitStatsComp`: энергия, броня, щиты, барьеры, стабилизация и разрешённые hit reactions.
- `CharacterControllerComp`: ссылка на `CustomCharacterController` для KinematicCharacterController-движения.
- `MoveableComp`: runtime/feature данные движения, включая `CustomCharacterController`, `KinematicCharacterMotor` и направления.
- `MoveableBehavioursComp`: набор movement behaviours (`Stable`, `Air`, `RootMotionStable`, `RootMotionAir`) и jump behaviour.
- `RegisterMoveableRequest`: запрос на инициализацию movement behaviours.
- `JumpableComp`: разрешает обработку прыжка.
- `MotionAnimationComp`: данные root motion/движения из анимации.
- `RegisterMotionAnimationRequest`: запрос на регистрацию motion animation данных.
- `AnimancerComp` или `AnimatorComp`: в зависимости от того, чем управляются анимации конкретного персонажа.
- `WeaponComp`: ссылка на weapon/hitbox, если персонаж может атаковать.
- `CombinableComp`: если персонаж использует combo-систему.
- `TargetingComp`: если персонаж может использовать soft/hard/manual targeting.
- `CameraFollowMeTransformComp`: если камера должна следовать за этим персонажем.
- `PushBoxCapsuleComp`: физический push/space occupancy коллайдер, если сущность участвует в столкновениях тел.
- `RigidBodyComp`: только если конкретная логика всё ещё требует Rigidbody-ссылку.

## Новый Враг

Базовый враг должен иметь:

- `TransformComp`: ссылка на transform сущности.
- `UnitStatsComp`: боевые характеристики и стабилизация.
- `CharacterControllerComp`: если враг двигается через KinematicCharacterController.
- `MoveableComp`: данные движения.
- `MoveableBehavioursComp`: movement behaviours врага.
- `RegisterMoveableRequest`: инициализация movement behaviours.
- `MotionAnimationComp`: если враг использует root motion или motion данные из анимации.
- `RegisterMotionAnimationRequest`: если нужен `MotionAnimationComp`.
- `AnimancerComp` или `AnimatorComp`: анимационная система врага.
- `WeaponComp`: если враг наносит урон через оружие/hitbox.
- `TargetFindableComp`: если игрок может брать врага в targeting.
- `PushBoxCapsuleComp`: если враг должен занимать место и сталкиваться телом.
- `FractionComp`: если нужно исключать friendly fire.

AI-компоненты врага пока не финализированы. Когда появится AI, сюда нужно добавить отдельный список обязательных feature-компонентов для поведения врага.

## HitBox / HurtBox

Для сущностей, которые наносят урон:

- На оружии должен быть `HitBoxMb`.
- В `HitBoxMb` должны быть указаны hit colliders.
- `UseEcsOverlap` обычно включён.
- `HurtBoxLayerMask` должен фильтровать только слои hurtbox'ов.
- Hit colliders включаются ECS-системой только во время `AttackConfig.HitWindow`.

Для сущностей, которые получают урон:

- На отдельном объекте hurtbox должен быть `HurtBoxMb`.
- Коллайдеры hurtbox'а должны быть на том же объекте, где стоит `HurtBoxMb`.
- По текущему правилу поиск `HurtBoxMb` у родителей не используется.

## AttackConfig

Каждая атака настраивает:

- `BaseDamage`: базовый урон.
- `BaseCritChance`: базовый шанс крита.
- `BaseCritMultiplier`: базовый множитель крита.
- `AnimationDurationSeconds`: желаемая длительность анимации атаки в секундах при `AttackSpeed = 1`. Если значение `0`, система использует реальную длину `AttackAnimation`.
- `AttackerMovement`: отдельный блок движения атакующего во время атаки. Не путать с `ReactionVelocity`, который описывает движение цели.
- `EndOfContinuousPart`: момент, после которого recovery можно прервать движением.
- `HitWindow`: окно активного hitbox'а.
- `ComboWindow`: окно перехода в следующую атаку комбо.
- `HitReactionType`: какую реакцию может вызвать атака при пробитии стабилизации.
- `ReactionVelocity`: направление/сила будущей реакции; сейчас данные сохраняются, но полноценное движение реакции ещё предстоит реализовать.
- `HitReactionDuration`: длительность состояния `InHitReactionComp`.

`HitReactionType.None` означает: атака не создаёт hit reaction. При этом урон по стабилизации всё равно должен списываться.

Реальная длительность атаки считается так:

```text
EffectiveAttackDuration = AttackConfig.AnimationDurationSeconds / UnitStatsComp.AttackSpeed
```

Если `AnimationDurationSeconds` не задан, вместо него берётся `AttackAnimation.length`. Скорость проигрывания `AnimancerState` выставляется автоматически:

```text
AnimancerSpeed = AttackAnimation.length / BaseAttackDuration * UnitStatsComp.AttackSpeed
```

Окна `HitWindow`, `ComboWindow` и `EndOfContinuousPart` остаются нормализованными значениями `0..1`, но теперь считаются от `EffectiveAttackDuration`, а не напрямую от длины клипа.

`AttackerMovement` используется так:

- `Mode`: какой movement behaviour использовать во время movement window атаки.
- `EndNormalizedTime`: когда атака перестаёт управлять movement. Если `0`, используется `EndOfContinuousPart`.
- `UseVerticalRootMotion`: разрешает вертикальную составляющую root motion.
- `ForceUngroundOnStart`: насильно отрывает атакующего от земли в начале movement window.
- `AirControl`: слабый контроль движения во время launcher/root-motion атаки.

Для launcher-атак вроде `Attack_Bash_1.2` используется `LauncherRootMotion`: vertical root motion применяется отдельным `AttackLauncherRootMotionBehaviour`, а после `EndNormalizedTime` персонаж возвращается в обычный `AirMovementBehaviour` или `StableMovementBehaviour`.

## ComboConfig_v2

Каждый узел комбо (`ComboConfig_v2`) настраивает, какой `AttackConfig` будет запущен и при каких условиях ввода он доступен.

- `StanceCondition`: ограничение по текущему состоянию атакующего. `Any` разрешает удар всегда, `GroundedOnly` только когда `CustomCharacterController.Motor.GroundingStatus.IsStableOnGround == true`, `AirborneOnly` только когда это значение `false`.
- Если stance не подходит, узел комбо не попадает в `AvailableCombos` и не может быть выбран даже из устаревшего буфера. Другой подходящий узел с тем же input может быть выбран как fallback.
- Старые ассеты без ручной настройки используют дефолт `Any`.

## UnitStatsComp

Ключевые боевые поля:

- `EnergyMax` / `EnergyCurrent`: единый ресурс здоровья и энергии.
- `Armor`: 1 единица брони блокирует 1 единицу урона.
- `ShieldMax` / `ShieldCurrent`: щиты принимают урон раньше энергии.
- `BarrierMax` / `BarrierCurrent`: барьеры принимают удар раньше щитов; 1 барьер поглощает весь удар.
- `Stabilization`: атакующая характеристика для расчёта шанса крита.
- `ClockSpeed`: атакующая характеристика для критического урона.
- `AttackSpeed`: множитель скорости атак. `1` — базовая скорость, `0.5` — в два раза медленнее, `2` — в два раза быстрее. Если в инспекторе оставить `0`, код выставит дефолт `1`.
- `StabilizationMax`: максимальная боевая стабилизация цели.
- `StabilizationCurrent`: текущая боевая стабилизация цели.
- `StabilizationRecoveryDelay`: задержка перед восстановлением стабилизации после урона.
- `StabilizationRecoveryTime`: время полного восстановления стабилизации от нуля до максимума.
- `AllowedHitReactions`: какие реакции разрешены для этой цели.

Если `AllowedHitReactions` равен `None`, цель не будет получать hit reaction, но стабилизация всё равно может списываться.

## CharacterControllerComp

`CharacterControllerComp` заменил старый `SlayerJetCharacterControllerComp`. Он нужен не только игроку, но и будущим врагам, которые будут двигаться через `CustomCharacterController`.

Текущая player-only система:

- `UpdatePlayerCharacterControllerSystem` работает только с сущностями, у которых есть `PlayerControlledComp`.
- Для врагов нужна отдельная система, которая будет передавать AI-направление движения/взгляда в тот же `CharacterControllerComp`.

## Минимальный Чеклист Врага

1. Создать prefab/entity с baker'ом.
2. Добавить `TransformComp`.
3. Добавить `UnitStatsComp` и настроить энергию, броню, стабилизацию, `AllowedHitReactions`.
4. Добавить `TargetFindableComp`, если враг должен попадать в targeting.
5. Добавить `HurtBoxMb` на отдельный hurtbox-объект с коллайдерами.
6. Добавить `FractionComp`, если нужна проверка friendly fire.
7. Добавить `CharacterControllerComp`, `MoveableComp`, `MoveableBehavioursComp`, `RegisterMoveableRequest`, если враг должен двигаться через KinematicCharacterController.
8. Добавить анимационные компоненты, если враг должен проигрывать анимации.
9. Добавить `WeaponComp` и `HitBoxMb`, если враг должен атаковать.

## Минимальный Чеклист Главного Героя

1. Создать prefab/entity с player baker'ом.
2. Добавить `PlayerControlledComp`.
3. Добавить `BaseInputControlsComp` на input-сущность/соответствующий объект, если требуется ввод.
4. Добавить `UnitStatsComp`.
5. Добавить movement-набор: `CharacterControllerComp`, `MoveableComp`, `MoveableBehavioursComp`, `RegisterMoveableRequest`, `JumpableComp`.
6. Добавить animation/root motion-набор: `MotionAnimationComp`, `RegisterMotionAnimationRequest`, `AnimancerComp` или `AnimatorComp`.
7. Добавить combat-набор: `WeaponComp`, `CombinableComp`, `TargetingComp`.
8. Добавить camera-набор: `CameraFollowMeTransformComp`.
9. Проверить `HitBoxMb`, `HurtBoxMb`, слои и layer masks.

## Что Обновлять При Новых Фичах

При добавлении новой gameplay-фичи нужно обновить этот документ или создать отдельный документ в `Docs/` и сослаться на него отсюда.

Обновлять нужно:

- обязательные компоненты для игрока;
- обязательные компоненты для врагов;
- новые настройки в config/inspector;
- новые runtime-системы и их роль;
- ограничения и known issues;
- порядок ручной настройки prefab/entity.

## Hit Reaction Movement

Текущая реализация реакций использует `HitReactionMovementBehaviour`. Он временно перехватывает `CustomCharacterController` цели, пока на сущности висит `InHitReactionComp`.

Флоу реакции:

1. `ApplyStabilizationDamageSystem` списывает стабилизацию цели.
2. Если стабилизация пробита и `HitReactionType` не `None`, создаётся `HitReactionRequest`.
3. `ApplyHitReactionRequestSystem` создаёт/обновляет `InHitReactionComp` и переводит `ReactionVelocity` из локальной настройки атаки в world velocity.
4. `ApplyHitReactionMovementSystem` переключает цель на `HitReactionMovementBehaviour`.
5. `InHitReactionComponentDeletingSystem` удаляет `InHitReactionComp`, когда реакция закончилась.

`ReactionVelocity` трактуется так:

- `x`: сила отталкивания по направлению от атакующего к центру реакции цели.
- `y`: вертикальная сила реакции.

Центр реакции цели задаётся в `UnitStatsComp.ReactionCenter`. Если он не указан, используется `TransformComp.Value.position`. Это нужно, чтобы направление реакции считалось не от pivot'а prefab'а, а от нормальной точки центра массы/корпуса.

Поведение типов реакций:

- `StaggerAndAirJuggle`: применяет `ReactionVelocity`, поэтому можно делать как полную остановку `(0, 0)`, так и слабое смещение.
- `Knockback`: применяет `ReactionVelocity` и на земле, и в воздухе.
- `Launch`: применяет `ReactionVelocity`, обычно с положительным `y`.
- `Knockdown`: на земле вводит цель в состояние knockdown на `HitReactionDuration`; в воздухе заставляет цель лететь вниз, а таймер knockdown стартует только после приземления.
- `None`: не создаёт реакцию, но урон по стабилизации всё равно списывается.

Если на префабе в `MoveableBehavioursComp` нет `HitReactionMovementBehaviour`, `RegisterMoveableRequestSystem` создаст его с дефолтными кодовыми значениями. Чтобы редактировать параметры поведения вручную в инспекторе, можно добавить `HitReactionMovementBehaviour` в список movement behaviours сущности.

## Character Controller Update

`UpdateCharacterControllerSystem` является общей системой обновления `CustomCharacterController` для игрока, врагов и dummy.

Система не читает input напрямую. Она только выбирает актуальный movement behaviour и передаёт в контроллер значения из `MoveableComp`:

- `NormalizedMoveDirection`
- `NormalizedLookDirection`

Источник этих значений должен быть отдельным:

- для игрока: input/player action системы;
- для врагов: будущие AI системы;
- для dummy: значения могут оставаться нулевыми.

Приоритет выбора movement behaviour:

1. `InHitReactionComp` -> `HitReactionMovementBehaviour`.
2. `AttackMovementLockComp` + grounded -> `RootMotionStableMovementBehaviour`.
3. `AttackMovementLockComp` + airborne -> `RootMotionAirMovementBehaviour`.
4. Grounded -> `StableMovementBehaviour`.
5. Airborne -> `AirMovementBehaviour`.

Это важно для launch/juggle: когда `InHitReactionComp` заканчивается, dummy/enemy больше не остаётся в `HitReactionMovementBehaviour`, а автоматически переходит в `AirMovementBehaviour` или `StableMovementBehaviour`.
